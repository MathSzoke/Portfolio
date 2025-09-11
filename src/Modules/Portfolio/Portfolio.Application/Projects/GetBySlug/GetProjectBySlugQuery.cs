using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Projects.GetBySlug;

public sealed record GetProjectBySlugQuery(string Slug) : IQuery<ProjectDetailsResponse>;

public sealed record ProjectDetailsResponse(
    Guid Id,
    string Slug,
    string Name,
    string Summary,
    string? DescriptionMarkdown,
    string? RepoUrl,
    string? LiveUrl,
    string? IconUrl,
    bool IsFeatured,
    int SortOrder,
    string Source,
    IReadOnlyList<string> Technologies,
    IReadOnlyList<string> Tags,
    IReadOnlyList<ProjectAssetItem> ProjectAssetItems,
    ShowcaseItem? ShowcaseItem
);

public sealed record ProjectAssetItem(Guid Id, string Url, string Kind, int SortOrder);
public sealed record ShowcaseItem(string ServiceName, string? EndpointUrl, string? HealthUrl);