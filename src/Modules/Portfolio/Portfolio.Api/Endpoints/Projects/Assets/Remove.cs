using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.Projects;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Assets.Remove;

namespace Portfolio.Api.Endpoints.Projects.Assets;

internal sealed class Remove : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/Projects/{slug}/assets", async (
                [FromRoute] string slug,
                [FromBody] AddAssetRequest request,
                [FromServices] ICommandHandler<RemoveAssetCommand> handler,
                CancellationToken ct) =>
            {
                var cmd = request.ToCommand<RemoveAssetCommand>();
                var result = await handler.Handle(cmd, ct);
                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .WithTags(Tags.Projects)
            .RequireAuthorization("SuperAdminOnly");
    }
}
