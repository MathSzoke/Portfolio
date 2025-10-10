using DeviceDetectorNET.Class;
using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.AI;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Abstractions.Presence;
using Portfolio.Application.Abstractions.Realtime;
using Portfolio.Domain.Chats;
using Portfolio.Domain.Chats.Enums;
using Portfolio.Domain.Users.Enums;
using SharedKernel;

namespace Portfolio.Application.Chat.Messages.Post;

internal sealed class PostMessageCommandHandler(
    IApplicationDbContext db,
    ICurrentUserContext currentUser,
    PresenceTracker presence,
    IAgentResponder agent,
    IChatNotifier notifier
) : ICommandHandler<PostMessageCommand, ChatMessageResponse>
{
    public async Task<Result<ChatMessageResponse>> Handle(PostMessageCommand cmd, CancellationToken ct)
    {
        var session = await db.ChatSessions.FirstOrDefaultAsync(x => x.Id == cmd.SessionId, ct);
        if (session is null) return Result.Failure<ChatMessageResponse>(ChatErrors.SessionNotFound(cmd.SessionId));
        if (session.Status != SessionStatus.Open) return Result.Failure<ChatMessageResponse>(ChatErrors.MessageNotAllowedInClosedSession);

        var currentUserId = currentUser.UserIdGuid;
        if (currentUserId == Guid.Empty) return Result.Failure<ChatMessageResponse>(ChatErrors.Unauthorized);

        var isRecipient = currentUserId == session.RecipientId;
        var senderEnum = cmd.AsMe || isRecipient ? Sender.Me : Sender.Visitor;

        var message = new ChatMessage
        {
            SessionId = session.Id,
            Content = cmd.Content,
            Sender = senderEnum,
            SenderUserId = currentUserId
        };

        db.ChatMessages.Add(message);

        if (isRecipient)
            session.LastRecipientSeenAt = DateTime.UtcNow;
        else
            session.LastSenderSeenAt = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);

        var dto = new ChatMessageResponse(message.Id, message.SessionId, message.Content, message.Sender.ToString(), message.SenderUserId, message.CreatedAt, message.ReadAt);
        await notifier.SendMessageToSessionAsync(session.Id, message.Id, message.Content, message.Sender.ToString(), message.SenderUserId, message.CreatedAt, message.ReadAt, ct);

        if (!isRecipient)
        {
            var recipientOnline = await presence.IsOnline(session.RecipientId.ToString());
            var recipientUser = await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == session.RecipientId, ct);
            var recipientStatus = recipientUser?.Status ?? UserStatus.Offline;
            var shouldAgentReply = !recipientOnline || recipientStatus != UserStatus.Available;

            if (shouldAgentReply)
            {
                var history = await db.ChatMessages
                    .AsNoTracking()
                    .Where(x => x.SessionId == session.Id && x.Id != message.Id)
                    .OrderBy(x => x.CreatedAt)
                    .Select(x => new ChatTurn(x.Sender == Sender.Visitor ? "user" : "assistant", x.Content))
                    .ToListAsync(ct);

                var replyText = await agent.GenerateReplyAsync(history, cmd.Content, ct);

                if (!string.IsNullOrWhiteSpace(replyText))
                {
                    var bot = new ChatMessage
                    {
                        SessionId = session.Id,
                        Content = replyText,
                        Sender = Sender.System,
                        SenderUserId = session.RecipientId
                    };

                    db.ChatMessages.Add(bot);
                    session.LastRecipientSeenAt = DateTime.UtcNow;
                    await db.SaveChangesAsync(ct);

                    await notifier.SendMessageToSessionAsync(session.Id, bot.Id, bot.Content, bot.Sender.ToString(), bot.SenderUserId, bot.CreatedAt, bot.ReadAt, ct);
                }
            }
        }

        return Result.Success(dto);
    }
}
