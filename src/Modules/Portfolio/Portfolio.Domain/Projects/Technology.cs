using SharedKernel;

namespace Portfolio.Domain.Projects;

public class Technology : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<ProjectTechnology> Projects { get; set; } = [];
}