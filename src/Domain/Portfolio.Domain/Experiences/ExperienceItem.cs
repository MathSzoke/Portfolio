using SharedKernel;

namespace Portfolio.Domain.Experiences;

public sealed class ExperienceItem : Entity
{
    public Guid Id { get; set; }
    public string Company { get; set; } = null!;
    public string LogoUrl { get; set; } = null!;
    public string RolePtBr { get; set; } = null!;
    public string RoleEnUs { get; set; } = null!;
    public string PeriodPtBr { get; set; } = null!;
    public string PeriodEnUs { get; set; } = null!;
    public string LocationPtBr { get; set; } = null!;
    public string LocationEnUs { get; set; } = null!;
    public string[] DescriptionPtBr { get; set; } = [];
    public string[] DescriptionEnUs { get; set; } = [];
    public string[] Techs { get; set; } = [];
    public string? StartDate { get; set; }
    public int SortOrder { get; set; }
}
