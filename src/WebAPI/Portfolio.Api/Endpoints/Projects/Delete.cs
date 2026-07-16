using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Delete;

namespace Portfolio.Api.Endpoints.Projects;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/Projects/{id:guid}", async (
                [FromRoute] Guid id,
                [FromServices] ICommandHandler<DeleteProjectCommand> handler,
                CancellationToken ct) =>
            {
                var cmd = new DeleteProjectCommand(id);
                var result = await handler.Handle(cmd, ct);
                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .WithTags(Tags.Projects)
            .RequireAuthorization("SuperAdminOnly");
    }
}