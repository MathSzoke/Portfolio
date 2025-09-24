using SharedKernel;

namespace Portfolio.Domain.Projects;

public sealed class ProjectRating : Entity
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public float Value { get; set; }
    public Project Project { get; set; } = null!;
}