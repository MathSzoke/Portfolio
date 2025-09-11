using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Projects.Enums;
using SharedKernel;

namespace Portfolio.Application.Projects.List;

internal sealed class ListProjectsQueryHandler(IApplicationDbContext db)
    : IQueryHandler<ListProjectsQuery, ProjectsPageResponse>
{
    public async Task<Result<ProjectsPageResponse>> Handle(ListProjectsQuery q, CancellationToken ct)
    {
        var query = db.Projects.AsQueryable();

        if (!string.IsNullOrWhiteSpace(q.Source) && Enum.TryParse<ProjectSource>(q.Source, true, out var src))
            query = query.Where(p => p.Source == src);

        if (q.Featured.HasValue)
            query = query.Where(p => p.IsFeatured == q.Featured.Value);

        if (!string.IsNullOrWhiteSpace(q.Search))
        {
            var s = q.Search.Trim();
            query = query.Where(p => p.Name.Contains(s) || p.Summary.Contains(s) || p.Slug.Contains(s));
        }

        if (!string.IsNullOrWhiteSpace(q.Tech))
            query = query.Where(p => p.Technologies.Any(t => t.Technology.Name == q.Tech));

        if (!string.IsNullOrWhiteSpace(q.Tag))
            query = query.Where(p => p.Tags.Any(t => t.Tag.Name == q.Tag));

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(p => p.IsFeatured)
            .ThenBy(p => p.SortOrder)
            .ThenBy(p => p.Name)
            .Skip((q.Page - 1) * q.PageSize)
            .Take(q.PageSize)
            .Select(p => new ProjectListItem(p.Id, p.Slug, p.Name, p.Summary, p.IconUrl, p.Source.ToString(), p.IsFeatured))
            .AsNoTracking()
            .ToListAsync(ct);

        return Result.Success(new ProjectsPageResponse(q.Page, q.PageSize, total, items));
    }
}