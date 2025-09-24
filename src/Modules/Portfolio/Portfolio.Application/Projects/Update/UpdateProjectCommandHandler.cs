using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Add;
using Portfolio.Domain.Projects;
using SharedKernel;

namespace Portfolio.Application.Projects.Update;

internal sealed class UpdateProjectCommandHandler(IApplicationDbContext db)
    : ICommandHandler<UpdateProjectCommand, ProjectResponse>
{
    public async Task<Result<ProjectResponse>> Handle(UpdateProjectCommand cmd, CancellationToken ct)
    {
        var project = await db.Projects
            .Include(p => p.Technologies).ThenInclude(pt => pt.Technology)
            .FirstOrDefaultAsync(p => p.Id == cmd.Id, ct);

        if (project is null)
            return Result.Failure<ProjectResponse>(ProjectErrors.NotFound(cmd.Id));

        project.Name = cmd.Name;
        project.Summary = cmd.Summary;
        project.ThumbnailUrl = cmd.ThumbnailUrl;
        project.ProjectUrl = cmd.ProjectUrl;
        project.RepoName = cmd.RepoName;
        project.SortOrder = cmd.SortOrder;

        var names = (cmd.Technologies ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var existingTechs = await db.Technologies
            .Where(t => names.Contains(t.Name))
            .ToListAsync(ct);

        var techDict = existingTechs.ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);

        var techsToRemove = project.Technologies
            .Where(pt => !names.Contains(pt.Technology.Name, StringComparer.OrdinalIgnoreCase))
            .ToList();
        db.ProjectTechnologies.RemoveRange(techsToRemove);

        var currentTechNames = project.Technologies
            .Where(pt => !techsToRemove.Contains(pt))
            .Select(pt => pt.Technology.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var name in names)
        {
            if (!currentTechNames.Contains(name))
            {
                if (!techDict.TryGetValue(name, out var tech))
                {
                    tech = new Technology { Name = name };
                    db.Technologies.Add(tech);
                    techDict[name] = tech;
                }

                project.Technologies.Add(new ProjectTechnology
                {
                    Project = project,
                    Technology = tech
                });
            }
        }

        await db.SaveChangesAsync(ct);

        return Result.Success(new ProjectResponse(project.Id));
    }
}
