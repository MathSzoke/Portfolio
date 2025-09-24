using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.Projects.Add;

public sealed record AddProjectCommand(
    string Name,
    string Summary,
    string ThumbnailUrl,
    string ProjectUrl,
    string RepoName,
    IReadOnlyCollection<string> Technologies,
    int SortOrder
) : ICommand<ProjectResponse>;