using SharedKernel;

namespace Portfolio.Domain.Projects;

public sealed class Showcase : Entity
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string ServiceName { get; set; } = null!;
    public string? EndpointUrl { get; set; }
    public string? HealthUrl { get; set; }

    public Project Project { get; set; } = null!;
}