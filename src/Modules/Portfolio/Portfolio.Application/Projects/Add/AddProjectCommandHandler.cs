using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Projects;
using Portfolio.Domain.Projects.Enums;
using SharedKernel;

namespace Portfolio.Application.Projects.Add;

internal sealed class AddProjectCommandHandler(IApplicationDbContext db)
    : ICommandHandler<AddProjectCommand, ProjectResponse>
{
    public async Task<Result<ProjectResponse>> Handle(AddProjectCommand cmd, CancellationToken ct)
    {
        if (await db.Projects.AnyAsync(p => p.Slug == cmd.Slug, ct))
            return Result.Failure<ProjectResponse>(Error.Failure(ProjectErrors.SlugNotUnique.Code, ProjectErrors.SlugNotUnique.Description));

        var project = BuildProject(cmd);

        await AttachTechnologiesAsync(project, cmd.Technologies, ct);
        await AttachTagsAsync(project, cmd.Tags, ct);
        AttachAssets(project, cmd.Assets);
        AttachShowcase(project, cmd.Source, cmd.Showcase);

        db.Projects.Add(project);
        await db.SaveChangesAsync(ct);

        return Result.Success(new ProjectResponse(project.Id, project.Slug));
    }

    private static Project BuildProject(AddProjectCommand cmd) =>
        new()
        {
            Id = Guid.NewGuid(),
            Slug = cmd.Slug,
            Name = cmd.Name,
            Summary = cmd.Summary,
            DescriptionMarkdown = cmd.DescriptionMarkdown,
            Source = cmd.Source,
            RepoUrl = cmd.RepoUrl,
            LiveUrl = cmd.LiveUrl,
            IconUrl = cmd.IconUrl,
            IsFeatured = false,
            SortOrder = 0
        };

    private static string[] NormalizeDistinct(IReadOnlyCollection<string>? list) =>
        (list ?? [])
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private async Task AttachTechnologiesAsync(Project project, IReadOnlyCollection<string>? techs, CancellationToken ct)
    {
        var names = NormalizeDistinct(techs);
        if (names.Length == 0) return;

        var existing = await db.Technologies
            .Where(t => names.Contains(t.Name))
            .ToListAsync(ct);

        var byName = existing.ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var n in names)
        {
            if (!byName.TryGetValue(n, out var tech))
            {
                tech = new Technology { Name = n };
                db.Technologies.Add(tech);
            }
            project.Technologies.Add(new ProjectTechnology { Project = project, Technology = tech });
        }
    }

    private async Task AttachTagsAsync(Project project, IReadOnlyCollection<string>? tags, CancellationToken ct)
    {
        var names = NormalizeDistinct(tags);
        if (names.Length == 0) return;

        var existing = await db.Tags
            .Where(t => names.Contains(t.Name))
            .ToListAsync(ct);

        var byName = existing.ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var n in names)
        {
            if (!byName.TryGetValue(n, out var tag))
            {
                tag = new Tag { Name = n };
                db.Tags.Add(tag);
            }
            project.Tags.Add(new ProjectTag { Project = project, Tag = tag });
        }
    }

    private static void AttachAssets(Project project, IReadOnlyCollection<string>? assets)
    {
        var urls = NormalizeDistinct(assets);
        for (var i = 0; i < urls.Length; i++)
        {
            project.Assets.Add(new ProjectAsset
            {
                Url = urls[i],
                Kind = AssetKind.Image,
                SortOrder = i
            });
        }
    }

    private static void AttachShowcase(Project project, ProjectSource source, ShowcaseDto? s)
    {
        if (source != ProjectSource.Aspire || s is null) return;

        project.Showcase = new Domain.Projects.Showcase
        {
            ServiceName = s.ServiceName,
            EndpointUrl = s.EndpointUrl,
            HealthUrl = s.HealthUrl
        };
    }
}
