using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.Projects;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Assets.Add;

namespace Portfolio.Api.Endpoints.Projects.Assets;

internal sealed class Add : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/Projects/{slug}/assets", async (
                [FromRoute] string slug,
                [FromBody] AddAssetRequest request,
                [FromServices] ICommandHandler<AddAssetCommand, AssetResponse> handler,
                CancellationToken ct) =>
            {
                var cmd = request.ToCommand<AddAssetCommand>();
                var result = await handler.Handle(cmd, ct);
                return result.Match(
                    _ => Results.NoContent(),
                    CustomResults.Problem);
            })
            .WithTags(Tags.Projects)
            .RequireAuthorization("SuperAdminOnly");
    }
}
