using SharedKernel;

namespace Portfolio.Domain.Curriculum;

public sealed class CurriculumAsset : Entity
{
    public Guid Id { get; set; }
    public string Language { get; set; } = null!;
    public string Url { get; set; } = null!;
}
