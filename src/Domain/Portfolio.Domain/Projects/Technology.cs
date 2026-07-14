using SharedKernel;

namespace Portfolio.Domain.Projects;

public sealed class Technology : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}
