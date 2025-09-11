using SharedKernel;

namespace Portfolio.Domain.Projects;

public sealed class Tag : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<ProjectTag> Projects { get; set; } = [];
}