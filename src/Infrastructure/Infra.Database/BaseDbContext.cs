using Microsoft.EntityFrameworkCore;
using SharedKernel;
using SharedKernel.DomainEvents;

namespace Infra.Database;

public abstract class BaseDbContext(
    DbContextOptions options,
    ICurrentUserContext? currentUserContext,
    DomainEventsDispatcher? domainEventsDispatcher) : DbContext(options)
{
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = currentUserContext?.UserId;

        foreach (var entry in ChangeTracker.Entries<Miscellaneous>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = currentUserId;
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = currentUserId;
                    entry.Entity.UpdatedAt = entry.Entity.CreatedAt;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedBy = currentUserId;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedBy = currentUserId;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        var domainEvents = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count != 0)
            .SelectMany(e =>
            {
                var events = e.DomainEvents.ToList();
                e.ClearDomainEvents();
                return events;
            })
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (domainEventsDispatcher is not null && domainEvents.Count != 0)
            await domainEventsDispatcher.DispatchAsync(domainEvents, cancellationToken);

        return result;
    }
}