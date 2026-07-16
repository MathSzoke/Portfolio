using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Chats;
using Portfolio.Domain.Chats.Enums;
using SharedKernel;

namespace Portfolio.Application.Chat.Sessions.Status;

internal sealed class CloseSessionCommandHandler(IApplicationDbContext db) : ICommandHandler<CloseSessionCommand>
{
    public async Task<Result> Handle(CloseSessionCommand cmd, CancellationToken ct)
    {
        var s = await db.ChatSessions.FirstOrDefaultAsync(x => x.Id == cmd.SessionId, ct);
        if (s is null) return Result.Failure(ChatErrors.SessionNotFound(cmd.SessionId));

        s.Close();
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}

internal sealed class ArchiveSessionCommandHandler(IApplicationDbContext db) : ICommandHandler<ArchiveSessionCommand>
{
    public async Task<Result> Handle(ArchiveSessionCommand cmd, CancellationToken ct)
    {
        var s = await db.ChatSessions.FirstOrDefaultAsync(x => x.Id == cmd.SessionId, ct);
        if (s is null) return Result.Failure(ChatErrors.SessionNotFound(cmd.SessionId));

        s.Archive();
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}

internal sealed class ReopenSessionCommandHandler(IApplicationDbContext db) : ICommandHandler<ReopenSessionCommand>
{
    public async Task<Result> Handle(ReopenSessionCommand cmd, CancellationToken ct)
    {
        var s = await db.ChatSessions.FirstOrDefaultAsync(x => x.Id == cmd.SessionId, ct);
        if (s is null) return Result.Failure(ChatErrors.SessionNotFound(cmd.SessionId));

        s.Reopen();
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }
}