using Portfolio.Domain.Projects.Enums;

namespace Portfolio.Api.Contracts.Projects;

public abstract class AddProjectRequest
{
    public string Slug { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Summary { get; init; } = null!;
    public string? DescriptionMarkdown { get; init; }
    public ProjectSource Source { get; init; } = ProjectSource.External;
    public string? RepoUrl { get; init; }
    public string? LiveUrl { get; init; }
    public string? IconUrl { get; init; }
    public IReadOnlyCollection<string> Technologies { get; init; } = [];
    public IReadOnlyCollection<string> Tags { get; init; } = [];
    public IReadOnlyCollection<string> Assets { get; init; } = [];
    public ShowcaseRequest? Showcase { get; init; }
    public float? Rating { get; init; }
}

public sealed class UpdateProjectRequest
{
    public string Name { get; init; } = null!;
    public string Summary { get; init; } = null!;
    public string? DescriptionMarkdown { get; init; }
    public string? RepoUrl { get; init; }
    public string? LiveUrl { get; init; }
    public string? IconUrl { get; init; }
    public IReadOnlyCollection<string> Technologies { get; init; } = [];
    public IReadOnlyCollection<string> Tags { get; init; } = [];
    public IReadOnlyCollection<string> Assets { get; init; } = [];
    public bool? IsFeatured { get; init; }
    public int? SortOrder { get; init; }
    public ShowcaseRequest? Showcase { get; init; }
    public float? Rating { get; init; }
}

public abstract class ShowcaseRequest
{
    public string ServiceName { get; init; } = null!;
    public string? EndpointUrl { get; init; }
    public string? HealthUrl { get; init; }
}

public abstract class ReorderProjectsRequest
{
    public IReadOnlyCollection<ReorderPair> Items { get; init; } = [];
    public abstract record ReorderPair(Guid Id, int SortOrder);
}

public sealed class AddAssetRequest
{
    public string Url { get; init; } = null!;
    public string? Title { get; init; }
}

public sealed class ReplaceShowcaseRequest
{
    public string ServiceName { get; init; } = null!;
    public string? EndpointUrl { get; init; }
    public string? HealthUrl { get; init; }
}
