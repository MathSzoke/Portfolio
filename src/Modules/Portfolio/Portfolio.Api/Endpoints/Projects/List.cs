using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.List;

namespace Portfolio.Api.Endpoints.Projects;

internal sealed class List : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/Projects", async (
                [FromQuery] string? source,
                [FromQuery] string? tag,
                [FromQuery] string? tech,
                [FromQuery] string? search,
                [FromQuery] bool? featured,
                [FromQuery] int page,
                [FromQuery] int pageSize,
                [FromServices] IQueryHandler<ListProjectsQuery, ProjectsPageResponse> handler,
                CancellationToken ct) =>
            {
                var p = page <= 0 ? 1 : page;
                var s = pageSize <= 0 ? 20 : pageSize;
                var query = new ListProjectsQuery(source, tag, tech, search, featured, p, s);
                var result = await handler.Handle(query, ct);
                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .WithTags(Tags.Projects);
    }
}