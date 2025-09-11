using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.Projects;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Add;

namespace Portfolio.Api.Endpoints.Projects;

internal sealed class Add : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/Projects", async (
                [FromBody] AddProjectRequest request,
                [FromServices] ICommandHandler<AddProjectCommand, ProjectResponse> handler,
                CancellationToken ct) =>
            {
                var cmd = request.ToCommand<AddProjectCommand>();
                var result = await handler.Handle(cmd, ct);

                return result.Match(
                    slug => Results.Ok(new { Slug = slug }),
                    CustomResults.Problem);
            })
            .WithTags(Tags.Projects)
            .RequireAuthorization("SuperAdminOnly");
    }
}