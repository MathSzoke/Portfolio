using SharedKernel;

namespace Portfolio.Domain.Projects;

public sealed class ProjectTag : Entity
{
    public Guid ProjectId { get; set; }
    public Guid TagId { get; set; }

    public Project Project { get; set; } = null!;
    public Tag Tag { get; set; } = null!;
}