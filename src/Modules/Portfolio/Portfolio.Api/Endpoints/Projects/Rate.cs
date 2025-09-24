using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.Projects;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Rate;

namespace Portfolio.Api.Endpoints.Projects;

internal sealed class RateProject : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/Projects/{id:guid}/rate", async (
                [FromRoute] Guid id,
                [FromBody] RateProjectRequest request,
                [FromServices] ICommandHandler<RateProjectCommand, RatedProjectResponse> handler,
                CancellationToken ct) =>
        {
            var cmd = new RateProjectCommand(id, request.Rating);
            var result = await handler.Handle(cmd, ct);
            return result.Match(
                Results.Ok,
                CustomResults.Problem);
        })
            .WithTags(Tags.Projects)
            .RequireAuthorization();
    }
}