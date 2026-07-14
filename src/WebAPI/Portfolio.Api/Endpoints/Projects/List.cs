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
                [FromServices] IQueryHandler<ListProjectsQuery, ProjectsPageResponse> handler,
                CancellationToken ct) =>
        {
            var query = new ListProjectsQuery(null, null, Page: 1, PageSize: int.MaxValue);
            var result = await handler.Handle(query, ct);

            return result.Match(
                Results.Ok,
                CustomResults.Problem
            );
        })
        .WithTags(Tags.Projects);
    }
}
