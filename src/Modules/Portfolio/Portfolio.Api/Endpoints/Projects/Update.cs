using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.Projects;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Update;
using Portfolio.Application.Projects.Add;

namespace Portfolio.Api.Endpoints.Projects;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/Projects/{slug}", async (
                [FromRoute] string slug,
                [FromBody] UpdateProjectRequest request,
                [FromServices] ICommandHandler<UpdateProjectCommand, ProjectResponse> handler,
                CancellationToken ct) =>
            {
                var cmd = request.ToCommand<UpdateProjectCommand>();
                var result = await handler.Handle(cmd, ct);
                return result.Match(
                    _ => Results.NoContent(),
                    CustomResults.Problem);
            })
            .WithTags(Tags.Projects)
            .RequireAuthorization("SuperAdminOnly");
    }
}
