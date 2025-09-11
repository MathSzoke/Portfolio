using Portfolio.Domain.Projects.Enums;
using SharedKernel;

namespace Portfolio.Domain.Projects;

public sealed class ProjectAsset : Entity
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public AssetKind Kind { get; set; } = AssetKind.Image;
    public string Url { get; set; } = null!;
    public string? Title { get; set; }
    public int SortOrder { get; set; } = 0;

    public Project Project { get; set; } = null!;
}