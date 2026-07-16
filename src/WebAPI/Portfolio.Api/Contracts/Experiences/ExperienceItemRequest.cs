namespace Portfolio.Api.Contracts.Experiences;

public sealed record ExperienceItemRequest(
    string SourceLanguage,
    string Company,
    string LogoUrl,
    string Role,
    string Period,
    string Location,
    string[] Description,
    string[] Techs,
    string? StartDate,
    int SortOrder);
