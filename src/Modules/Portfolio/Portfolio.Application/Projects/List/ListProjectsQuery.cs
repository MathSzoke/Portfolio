using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Projects.List;

public sealed record ListProjectsQuery(
    string? Source,
    string? Tag,
    string? Tech,
    string? Search,
    bool? Featured,
    int Page,
    int PageSize
) : IQuery<ProjectsPageResponse>;

public sealed record ProjectsPageResponse(int Page, int PageSize, int Total, IReadOnlyList<ProjectListItem> ProjectListItems);
public sealed record ProjectListItem(Guid Id, string Slug, string Name, string Summary, string? IconUrl, string Source, bool IsFeatured);