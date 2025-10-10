using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Abstractions.Presence;
using SharedKernel;

namespace Portfolio.Application.Presence.OwnerOnline;

internal sealed class OwnerOnlineQueryHandler(IApplicationDbContext db, PresenceTracker presence)
    : IQueryHandler<OwnerOnlineQuery, bool>
{
    public async Task<Result<bool>> Handle(OwnerOnlineQuery query, CancellationToken ct)
    {
        var owner = await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == query.OwnerEmail, ct);
        if (owner is null) return Result.Success(false);
        var online = await presence.IsOnline(owner.Id.ToString());
        return Result.Success(online);
    }
}
