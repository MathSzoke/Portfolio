using SharedKernel;

namespace Portfolio.Domain.Curriculum;

public sealed class CurriculumAsset : Entity
{
    public Guid Id { get; set; }
    public string Language { get; set; } = null!;
    public string? Url { get; set; }
    public byte[]? FileContent { get; set; }
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
}
