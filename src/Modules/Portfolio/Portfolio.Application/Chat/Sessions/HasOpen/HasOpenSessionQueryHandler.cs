using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using SharedKernel;

namespace Portfolio.Application.Chat.Sessions.HasOpen;

internal sealed class HasOpenSessionQueryHandler(IApplicationDbContext db) : IQueryHandler<HasOpenSessionQuery, bool>
{
    public async Task<Result<bool>> Handle(HasOpenSessionQuery query, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(query.Email)) return Result.Success(false);
        var has = await db.ChatSessions.AsNoTracking().AnyAsync(x => x.Status == Domain.Chats.Enums.SessionStatus.Open, ct);
        return Result.Success(has);
    }
}
