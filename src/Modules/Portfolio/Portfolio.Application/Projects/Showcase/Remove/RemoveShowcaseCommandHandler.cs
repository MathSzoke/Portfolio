using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using SharedKernel;

namespace Portfolio.Application.Projects.Showcase.Remove;

internal sealed class RemoveShowcaseCommandHandler(IApplicationDbContext db) : ICommandHandler<RemoveShowcaseCommand>
{
    public async Task<Result> Handle(RemoveShowcaseCommand cmd, CancellationToken ct)
    {
        var p = await db.Projects.Include(x => x.Showcase).FirstOrDefaultAsync(x => x.Slug == cmd.Slug, ct);
        if (p is null) return Result.Failure(Error.NotFound("project.not_found", $"Project '{cmd.Slug}' not found"));
        if (p.Showcase is null) return Result.Success();
        db.Showcases.Remove(p.Showcase);
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}