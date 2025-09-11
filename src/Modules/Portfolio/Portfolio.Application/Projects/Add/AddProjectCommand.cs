using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Projects.Enums;

namespace Portfolio.Application.Projects.Add;

public sealed record ShowcaseDto(string ServiceName, string? EndpointUrl, string? HealthUrl);

public sealed record AddProjectCommand(
    string Slug,
    string Name,
    string Summary,
    string? DescriptionMarkdown,
    ProjectSource Source,
    string? RepoUrl,
    string? LiveUrl,
    string? IconUrl,
    IReadOnlyCollection<string> Technologies,
    IReadOnlyCollection<string> Tags,
    IReadOnlyCollection<string> Assets,
    ShowcaseDto? Showcase
) : ICommand<ProjectResponse>;