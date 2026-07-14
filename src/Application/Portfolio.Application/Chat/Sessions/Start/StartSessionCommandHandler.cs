using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Chats;
using Portfolio.Domain.Chats.Enums;
using Portfolio.Domain.Users;
using SharedKernel;

namespace Portfolio.Application.Chat.Sessions.Start;

internal sealed class StartSessionCommandHandler(
    IApplicationDbContext db,
    ICurrentUserContext currentUser
) : ICommandHandler<StartSessionCommand, StartSessionResponse>
{
    public async Task<Result<StartSessionResponse>> Handle(StartSessionCommand cmd, CancellationToken ct)
    {
        var senderId = currentUser.UserIdGuid;
        if (senderId == Guid.Empty) return Result.Failure<StartSessionResponse>(ChatErrors.Unauthorized);
        if (cmd.RecipientId == Guid.Empty) return Result.Failure<StartSessionResponse>(ChatErrors.InvalidPayload);

        var existing = await db.ChatSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SenderId == senderId && x.RecipientId == cmd.RecipientId && x.Status == SessionStatus.Open, ct);

        if (existing is not null)
        {
            return Result.Success(new StartSessionResponse(existing.Id, existing.SenderId, existing.RecipientId, existing.Status.ToString(), existing.CreatedAt));
        }

        var session = ChatSession.Start(Guid.NewGuid(), senderId, cmd.RecipientId);
        db.ChatSessions.Add(session);

        await db.SaveChangesAsync(ct);

        return Result.Success(new StartSessionResponse(session.Id, session.SenderId, session.RecipientId, session.Status.ToString(), session.CreatedAt));
    }
}
