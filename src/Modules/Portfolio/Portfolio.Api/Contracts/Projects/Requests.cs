namespace Portfolio.Api.Contracts.Projects;

public sealed class AddProjectRequest
{
    public string Name { get; init; } = null!;
    public string Summary { get; init; } = null!;
    public string ThumbnailUrl { get; init; } = null!;
    public string ProjectUrl { get; init; } = null!;
    public string RepoName { get; init; } = null!;
    public IReadOnlyCollection<string> Technologies { get; init; } = [];
    public int SortOrder { get; init; } = 0;
}

public sealed class UpdateProjectRequest
{
    public string Name { get; init; } = null!;
    public string Summary { get; init; } = null!;
    public string ThumbnailUrl { get; init; } = null!;
    public string ProjectUrl { get; init; } = null!;
    public string RepoName { get; init; } = null!;
    public IReadOnlyCollection<string> Technologies { get; init; } = [];
    public int SortOrder { get; init; } = 0;
}

public sealed class ReorderProjectsRequest
{
    public IReadOnlyCollection<ReorderPair> Items { get; init; } = [];

    public sealed record ReorderPair(Guid Id, int SortOrder);
}

public sealed class RateProjectRequest
{
    public float Rating { get; init; }
}