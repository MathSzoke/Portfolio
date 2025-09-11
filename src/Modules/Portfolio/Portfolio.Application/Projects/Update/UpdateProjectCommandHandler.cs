using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Add;
using Portfolio.Domain.Projects;
using Portfolio.Domain.Projects.Enums;
using SharedKernel;

namespace Portfolio.Application.Projects.Update;

internal sealed class UpdateProjectCommandHandler(IApplicationDbContext db)
    : ICommandHandler<UpdateProjectCommand, ProjectResponse>
{
    public async Task<Result<ProjectResponse>> Handle(UpdateProjectCommand cmd, CancellationToken ct)
    {
        var project = await db.Projects
            .Include(p => p.Technologies).ThenInclude(pt => pt.Technology)
            .Include(p => p.Tags).ThenInclude(pt => pt.Tag)
            .Include(p => p.Assets)
            .Include(p => p.Showcase)
            .FirstOrDefaultAsync(p => p.Id == cmd.Id, ct);

        if (project is null) return Result.Failure<ProjectResponse>(ProjectErrors.NotFound(cmd.Id));

        project.Name = cmd.Name;
        project.Summary = cmd.Summary;
        project.DescriptionMarkdown = cmd.DescriptionMarkdown;
        project.RepoUrl = cmd.RepoUrl;
        project.LiveUrl = cmd.LiveUrl;
        project.IconUrl = cmd.IconUrl;
        if (cmd.IsFeatured.HasValue) project.IsFeatured = cmd.IsFeatured.Value;
        if (cmd.SortOrder.HasValue) project.SortOrder = cmd.SortOrder.Value;

        if (cmd.Technologies is { Count: > 0 })
        {
            project.Technologies.Clear();
            var names = cmd.Technologies.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            var existing = await db.Technologies.Where(t => names.Contains(t.Name)).ToListAsync(ct);
            var dict = existing.ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);
            foreach (var n in names)
            {
                if (!dict.TryGetValue(n, out var tech))
                {
                    tech = new Technology { Name = n };
                    db.Technologies.Add(tech);
                }
                project.Technologies.Add(new ProjectTechnology { Project = project, Technology = tech });
            }
        }

        if (cmd.Tags is { Count: > 0 })
        {
            project.Tags.Clear();
            var names = cmd.Tags.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            var existing = await db.Tags.Where(t => names.Contains(t.Name)).ToListAsync(ct);
            var dict = existing.ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);
            foreach (var n in names)
            {
                if (!dict.TryGetValue(n, out var tag))
                {
                    tag = new Tag { Name = n };
                    db.Tags.Add(tag);
                }
                project.Tags.Add(new ProjectTag { Project = project, Tag = tag });
            }
        }

        if (cmd.Assets is { Count: > 0 })
        {
            project.Assets.Clear();
            var urls = cmd.Assets.Where(u => !string.IsNullOrWhiteSpace(u)).Select(u => u.Trim()).Distinct().ToArray();
            for (var i = 0; i < urls.Length; i++)
            {
                project.Assets.Add(new ProjectAsset { Url = urls[i], Kind = AssetKind.Image, SortOrder = i });
            }
        }

        if (cmd.Showcase is null)
        {
            if (project.Showcase is not null) db.Showcases.Remove(project.Showcase);
        }
        else
        {
            if (project.Showcase is null)
                project.Showcase = new Domain.Projects.Showcase { ServiceName = cmd.Showcase.ServiceName, EndpointUrl = cmd.Showcase.EndpointUrl, HealthUrl = cmd.Showcase.HealthUrl };
            else
            {
                project.Showcase.ServiceName = cmd.Showcase.ServiceName;
                project.Showcase.EndpointUrl = cmd.Showcase.EndpointUrl;
                project.Showcase.HealthUrl = cmd.Showcase.HealthUrl;
            }
        }

        await db.SaveChangesAsync(ct);
        return Result.Success(new ProjectResponse(project.Id, project.Slug));
    }
}
