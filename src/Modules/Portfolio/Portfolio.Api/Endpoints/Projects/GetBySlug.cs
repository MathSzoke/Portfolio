using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Add;
using Portfolio.Application.Projects.GetBySlug;

namespace Portfolio.Api.Endpoints.Projects;

internal sealed class GetBySlug : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/Projects/{slug}", async (
                [FromRoute] string slug,
                [FromServices] IQueryHandler<GetProjectBySlugQuery, ProjectResponse> handler,
                CancellationToken ct) =>
            {
                var query = new GetProjectBySlugQuery(slug);
                var result = await handler.Handle(query, ct);
                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .WithTags(Tags.Projects);
    }
}
