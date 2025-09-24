using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using SharedKernel;

namespace Portfolio.Application.Projects.List;

internal sealed class ListProjectsQueryHandler(IApplicationDbContext db)
    : IQueryHandler<ListProjectsQuery, ProjectsPageResponse>
{
    public async Task<Result<ProjectsPageResponse>> Handle(ListProjectsQuery q, CancellationToken ct)
    {
        var query = db.Projects.AsQueryable();

        if (!string.IsNullOrWhiteSpace(q.Search))
        {
            var s = q.Search.Trim();
            query = query.Where(p => p.Name.Contains(s) || p.Summary.Contains(s));
        }

        if (!string.IsNullOrWhiteSpace(q.Tech))
            query = query.Where(p => p.Technologies.Any(t => t.Technology.Name == q.Tech));

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderBy(p => p.SortOrder)
            .ThenBy(p => p.Name)
            .Skip((q.Page - 1) * q.PageSize)
            .Take(q.PageSize)
            .Select(p => new ProjectListItem(
                p.Id,
                p.Name,
                p.Summary,
                p.ThumbnailUrl,
                p.ProjectUrl,
                p.RepoName,
                p.Rating,
                p.RatingCount,
                p.SortOrder,
                p.Technologies.Select(t => t.Technology.Name).ToList()
            ))
            .AsNoTracking()
            .ToListAsync(ct);

        return Result.Success(new ProjectsPageResponse(q.Page, q.PageSize, total, items));
    }
}