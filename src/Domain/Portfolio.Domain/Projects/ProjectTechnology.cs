using SharedKernel;

namespace Portfolio.Domain.Projects;

public sealed class ProjectTechnology : Entity
{
    public Guid ProjectId { get; set; }
    public Guid TechnologyId { get; set; }

    public Project Project { get; set; } = null!;
    public Technology Technology { get; set; } = null!;
}
