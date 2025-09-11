using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;

namespace Portfolio.Infrastructure.Authorization;

internal sealed class PermissionProvider(IApplicationDbContext db)
{
    public async Task<HashSet<string>> GetForUserIdAsync(Guid userId)
    {
        var u = await db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
        var perms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (u is null) return perms;
        if (u.Roles?.Contains("SuperAdmin", StringComparer.OrdinalIgnoreCase) == true)
            perms.Add("SuperAdmin");
        return perms;
    }
}