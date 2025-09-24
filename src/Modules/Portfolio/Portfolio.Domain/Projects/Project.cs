using SharedKernel;

namespace Portfolio.Domain.Projects;

public sealed class Project : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string ThumbnailUrl { get; set; } = null!;
    public string ProjectUrl { get; set; } = null!;
    public string RepoName { get; set; } = null!;
    public float Rating { get; set; } = 0;
    public int RatingCount { get; set; } = 0;
    public int SortOrder { get; set; } = 0;

    public ICollection<ProjectTechnology> Technologies { get; set; } = [];
}