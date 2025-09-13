using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Add;

namespace Portfolio.Application.Projects.Update;

public sealed record UpdateProjectCommand(
    Guid Id,
    string Name,
    string Summary,
    string? DescriptionMarkdown,
    string? RepoUrl,
    string? LiveUrl,
    string? IconUrl,
    IReadOnlyCollection<string> Technologies,
    IReadOnlyCollection<string> Tags,
    IReadOnlyCollection<string> Assets,
    bool? IsFeatured,
    int? SortOrder,
    Add.ShowcaseDto? Showcase
) : ICommand<ProjectResponse>;