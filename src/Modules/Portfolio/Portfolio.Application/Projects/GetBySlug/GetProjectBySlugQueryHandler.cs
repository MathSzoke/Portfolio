using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Projects;
using SharedKernel;

namespace Portfolio.Application.Projects.GetBySlug;

internal sealed class GetProjectBySlugQueryHandler(IApplicationDbContext db)
    : IQueryHandler<GetProjectBySlugQuery, ProjectDetailsResponse>
{
    public async Task<Result<ProjectDetailsResponse>> Handle(GetProjectBySlugQuery q, CancellationToken ct)
    {
        var p = await db.Projects
            .Include(x => x.Technologies).ThenInclude(x => x.Technology)
            .Include(x => x.Tags).ThenInclude(x => x.Tag)
            .Include(x => x.Assets)
            .Include(x => x.Showcase)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Slug == q.Slug, ct);

        if (p is null) return Result.Failure<ProjectDetailsResponse>(ProjectErrors.NotFoundBySlug(q.Slug));

        var resp = new ProjectDetailsResponse(
            p.Id, p.Slug, p.Name, p.Summary, p.DescriptionMarkdown,
            p.RepoUrl, p.LiveUrl, p.IconUrl, p.IsFeatured, p.SortOrder, p.Source.ToString(),
            p.Technologies.Select(t => t.Technology.Name).ToList(),
            p.Tags.Select(t => t.Tag.Name).ToList(),
            p.Assets.OrderBy(a => a.SortOrder).Select(a => new ProjectAssetItem(a.Id, a.Url, a.Kind.ToString(), a.SortOrder)).ToList(),
            p.Showcase is null ? null : new ShowcaseItem(p.Showcase.ServiceName, p.Showcase.EndpointUrl, p.Showcase.HealthUrl)
        );

        return Result.Success(resp);
    }
}