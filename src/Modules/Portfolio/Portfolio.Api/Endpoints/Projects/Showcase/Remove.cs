using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Showcase.Remove;

namespace Portfolio.Api.Endpoints.Projects.Showcase;

internal sealed class Remove : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/Projects/{slug}/showcase", async (
                [FromRoute] string slug,
                [FromServices] ICommandHandler<RemoveShowcaseCommand> handler,
                CancellationToken ct) =>
            {
                var cmd = new RemoveShowcaseCommand(slug);
                var result = await handler.Handle(cmd, ct);
                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .WithTags(Tags.Projects)
            .RequireAuthorization("SuperAdminOnly");
    }
}