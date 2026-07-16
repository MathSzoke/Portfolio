using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Abstractions.Projects;
using Portfolio.Domain.Projects;
using SharedKernel;

namespace Portfolio.Application.Projects.Add;

internal sealed class AddProjectCommandHandler(
    IApplicationDbContext db,
    IStatusProjectsClient statusClient)
    : ICommandHandler<AddProjectCommand, ProjectResponse>
{
    public async Task<Result<ProjectResponse>> Handle(AddProjectCommand cmd, CancellationToken ct)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = cmd.Name,
            Summary = cmd.Summary,
            ThumbnailUrl = cmd.ThumbnailUrl,
            ProjectUrl = cmd.ProjectUrl,
            RepoName = cmd.RepoName,
            Rating = 0,
            SortOrder = cmd.SortOrder
        };

        await AttachTechnologiesAsync(project, cmd.Technologies, ct);

        db.Projects.Add(project);
        await db.SaveChangesAsync(ct);

        if (!string.IsNullOrWhiteSpace(cmd.UrlEndpoint))
        {
            try
            {
                await statusClient.RegisterAsync(
                    project.Name,
                    cmd.UrlEndpoint,
                    project.ProjectUrl,
                    ct
                );
            }
            catch
            {
            }
        }

        return Result.Success(new ProjectResponse(project.Id));
    }

    private async Task AttachTechnologiesAsync(Project project, IReadOnlyCollection<string>? techs, CancellationToken ct)
    {
        if (techs is null || techs.Count == 0) return;

        var names = techs
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var existing = await db.Technologies
            .Where(t => names.Contains(t.Name))
            .ToListAsync(ct);

        var dict = existing.ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var n in names)
        {
            if (!dict.TryGetValue(n, out var tech))
            {
                tech = new Technology { Name = n };
                db.Technologies.Add(tech);
            }

            project.Technologies.Add(new ProjectTechnology
            {
                Project = project,
                Technology = tech
            });
        }
    }
}