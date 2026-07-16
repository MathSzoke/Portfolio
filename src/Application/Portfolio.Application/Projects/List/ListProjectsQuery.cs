using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Projects.List;

public sealed record ListProjectsQuery(
    string? Tech,
    string? Search,
    int Page,
    int PageSize
) : IQuery<ProjectsPageResponse>;

public sealed record ProjectsPageResponse(
    int Page,
    int PageSize,
    int Total,
    IReadOnlyList<ProjectListItem> ProjectListItems
);

public sealed record ProjectListItem(
    Guid Id,
    string Name,
    string Summary,
    string ThumbnailUrl,
    string ProjectUrl,
    string RepoName,
    float Rating,
    int RatingCount,
    int SortOrder,
    IReadOnlyList<string> Technologies
);