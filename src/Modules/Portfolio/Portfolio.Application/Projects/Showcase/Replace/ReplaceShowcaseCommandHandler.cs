using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Projects.Enums;
using SharedKernel;

namespace Portfolio.Application.Projects.Showcase.Replace;

internal sealed class ReplaceShowcaseCommandHandler(IApplicationDbContext db) : ICommandHandler<ReplaceShowcaseCommand>
{
    public async Task<Result> Handle(ReplaceShowcaseCommand cmd, CancellationToken ct)
    {
        var p = await db.Projects.Include(x => x.Showcase).FirstOrDefaultAsync(x => x.Slug == cmd.Slug, ct);
        if (p is null) return Result.Failure(Domain.Projects.ProjectErrors.NotFoundBySlug(cmd.Slug));
        if (p.Source != ProjectSource.Aspire) return Result.Failure(Domain.Projects.ProjectErrors.ShowcaseNotAllowedForExternal);
        if (p.Showcase is null)
            p.Showcase = new Domain.Projects.Showcase { ServiceName = cmd.Showcase.ServiceName, EndpointUrl = cmd.Showcase.EndpointUrl, HealthUrl = cmd.Showcase.HealthUrl };
        else
        {
            p.Showcase.ServiceName = cmd.Showcase.ServiceName;
            p.Showcase.EndpointUrl = cmd.Showcase.EndpointUrl;
            p.Showcase.HealthUrl = cmd.Showcase.HealthUrl;
        }
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }
}