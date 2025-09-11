using Portfolio.Application.Projects.Add;
using Portfolio.Application.Projects.Assets.Add;
using Portfolio.Application.Projects.Assets.Remove;
using Portfolio.Application.Projects.Delete;
using Portfolio.Application.Projects.GetBySlug;
using Portfolio.Application.Projects.List;
using Portfolio.Application.Projects.Reorder;
using Portfolio.Application.Projects.Showcase.Remove;
using Portfolio.Application.Projects.Showcase.Replace;
using Portfolio.Application.Projects.Update;

namespace Portfolio.Api.Contracts.Projects;

public static class ProjectMappings
{
    private static readonly Dictionary<(Type Target, Type Args), Delegate> Factories = new()
    {
        [(typeof(AddProjectCommand), typeof(AddProjectRequest))] =
            (Func<AddProjectRequest, AddProjectCommand>)(r =>
                new AddProjectCommand(
                    r.Slug, r.Name, r.Summary, r.DescriptionMarkdown, r.Source, r.RepoUrl, r.LiveUrl, r.IconUrl,
                    r.Technologies,
                    r.Tags,
                    r.Assets,
                    r.Showcase is null ? null : new ShowcaseDto(r.Showcase.ServiceName, r.Showcase.EndpointUrl, r.Showcase.HealthUrl)
                )),

        [(typeof(UpdateProjectCommand), typeof(ValueTuple<Guid, UpdateProjectRequest>))] =
            (Func<(Guid, UpdateProjectRequest), UpdateProjectCommand>)(t =>
                new UpdateProjectCommand(
                    t.Item1,
                    t.Item2.Name,
                    t.Item2.Summary,
                    t.Item2.DescriptionMarkdown,
                    t.Item2.RepoUrl,
                    t.Item2.LiveUrl,
                    t.Item2.IconUrl,
                    t.Item2.Technologies,
                    t.Item2.Tags,
                    t.Item2.Assets,
                    t.Item2.IsFeatured,
                    t.Item2.SortOrder,
                    t.Item2.Showcase is null ? null : new ShowcaseDto(t.Item2.Showcase.ServiceName, t.Item2.Showcase.EndpointUrl, t.Item2.Showcase.HealthUrl)
                )),

        [(typeof(DeleteProjectCommand), typeof(Guid))] =
            (Func<Guid, DeleteProjectCommand>)(id => new DeleteProjectCommand(id)),

        [(typeof(GetProjectBySlugQuery), typeof(string))] =
            (Func<string, GetProjectBySlugQuery>)(slug => new GetProjectBySlugQuery(slug)),

        [(typeof(ListProjectsQuery), typeof(ProjectsQueryString))] =
            (Func<ProjectsQueryString, ListProjectsQuery>)(q =>
                new ListProjectsQuery(q.Source, q.Tag, q.Tech, q.Search, q.Featured, q.Page, q.PageSize)),

        [(typeof(ReorderProjectsCommand), typeof(ReorderProjectsRequest))] =
            (Func<ReorderProjectsRequest, ReorderProjectsCommand>)(r =>
                new ReorderProjectsCommand(r.Items.Select(i => new Item(i.Id, i.SortOrder)).ToArray())),

        [(typeof(AddAssetCommand), typeof(ValueTuple<Guid, AddAssetRequest>))] =
            (Func<(Guid, AddAssetRequest), AddAssetCommand>)(t =>
                new AddAssetCommand(t.Item1, t.Item2.Url, t.Item2.Title)),

        [(typeof(RemoveAssetCommand), typeof(ValueTuple<Guid, Guid>))] =
            (Func<(Guid, Guid), RemoveAssetCommand>)(t =>
                new RemoveAssetCommand(t.Item1, t.Item2)),

        [(typeof(ReplaceShowcaseCommand), typeof(ValueTuple<string, ReplaceShowcaseRequest>))] =
            (Func<(string, ReplaceShowcaseRequest), ReplaceShowcaseCommand>)(t =>
                new ReplaceShowcaseCommand(t.Item1, new ShowcaseDto(t.Item2.ServiceName, t.Item2.EndpointUrl, t.Item2.HealthUrl))),

        [(typeof(RemoveShowcaseCommand), typeof(string))] =
            (Func<string, RemoveShowcaseCommand>)(slug => new RemoveShowcaseCommand(slug)),
    };

    public static T ToCommand<T>(this object args)
    {
        ArgumentNullException.ThrowIfNull(args);
        var key = (typeof(T), args.GetType());
        if (!Factories.TryGetValue(key, out var factory))
            throw new NotSupportedException($"{typeof(T).Name} from {args.GetType().Name} is not mapped.");
        return (T)factory.DynamicInvoke(args)!;
    }
}

public abstract class ProjectsQueryString
{
    public string? Source { get; init; }
    public string? Tag { get; init; }
    public string? Tech { get; init; }
    public string? Search { get; init; }
    public bool? Featured { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
