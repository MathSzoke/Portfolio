namespace Portfolio.Api.Contracts.Experiences;

public sealed record ExperienceItemResponse(
    Guid Id,
    string Company,
    string Logo,
    string Role,
    string Period,
    string Location,
    string[] Description,
    string[] Techs,
    string? StartDate,
    int SortOrder,
    ExperienceItemTranslations Translations);

public sealed record ExperienceItemTranslations(
    ExperienceItemTranslation PtBr,
    ExperienceItemTranslation EnUs);

public sealed record ExperienceItemTranslation(
    string Role,
    string Period,
    string Location,
    string[] Description);
