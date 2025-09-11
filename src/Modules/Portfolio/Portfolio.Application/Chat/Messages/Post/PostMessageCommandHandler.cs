using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Chats;
using Portfolio.Domain.Chats.Enums;
using SharedKernel;

namespace Portfolio.Application.Chat.Messages.Post;

internal sealed class PostMessageCommandHandler(IApplicationDbContext db)
    : ICommandHandler<PostMessageCommand, ChatMessageResponse>
{
    public async Task<Result<ChatMessageResponse>> Handle(PostMessageCommand cmd, CancellationToken ct)
    {
        var s = await db.ChatSessions.FirstOrDefaultAsync(x => x.Id == cmd.SessionId, ct);
        if (s is null) return Result.Failure<ChatMessageResponse>(ChatErrors.SessionNotFound(cmd.SessionId));
        if (s.Status != SessionStatus.Open) return Result.Failure<ChatMessageResponse>(ChatErrors.MessageNotAllowedInClosedSession);
        var sender = cmd.AsMe ? Sender.Me : Sender.Visitor;
        var m = new ChatMessage { SessionId = s.Id, Content = cmd.Content, Sender = sender };
        db.ChatMessages.Add(m);
        s.LastSeenAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success(new ChatMessageResponse(m.Id, m.SessionId, m.Content, m.Sender.ToString(), m.CreatedAt, m.ReadAt));
    }
}