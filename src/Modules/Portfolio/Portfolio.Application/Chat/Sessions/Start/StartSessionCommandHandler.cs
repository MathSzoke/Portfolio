using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Chats;
using Portfolio.Domain.Chats.Enums;
using SharedKernel;

namespace Portfolio.Application.Chat.Sessions.Start;

internal sealed class StartSessionCommandHandler(IApplicationDbContext db)
    : ICommandHandler<StartSessionCommand, ChatSessionResponse>
{
    public async Task<Result<ChatSessionResponse>> Handle(StartSessionCommand cmd, CancellationToken ct)
    {
        var s = new ChatSession { Name = cmd.Name, Email = cmd.Email, Status = SessionStatus.Open };
        db.ChatSessions.Add(s);
        await db.SaveChangesAsync(ct);
        return Result.Success(new ChatSessionResponse(s.Id, s.Name!, s.Email!, s.Status.ToString(), s.CreatedAt));
    }
}