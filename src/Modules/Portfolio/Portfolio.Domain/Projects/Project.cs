using Portfolio.Domain.Projects.Enums;
using SharedKernel;

namespace Portfolio.Domain.Projects;

public sealed class Project : Entity
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public float Rating { get; set; } = 0;
    public string? DescriptionMarkdown { get; set; }
    public ProjectSource Source { get; set; } = ProjectSource.External;
    public string? RepoUrl { get; set; }
    public string? LiveUrl { get; set; }
    public string? IconUrl { get; set; }
    public bool IsFeatured { get; set; } = false;
    public int SortOrder { get; set; } = 0;

    public ICollection<ProjectTechnology> Technologies { get; set; } = [];
    public ICollection<ProjectTag> Tags { get; set; } = [];
    public ICollection<ProjectAsset> Assets { get; set; } = [];
    public Showcase? Showcase { get; set; }
}