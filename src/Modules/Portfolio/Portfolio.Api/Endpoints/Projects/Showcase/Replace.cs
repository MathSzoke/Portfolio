using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.Projects;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Add;
using Portfolio.Application.Projects.Showcase.Replace;

namespace Portfolio.Api.Endpoints.Projects.Showcase;

internal sealed class Replace : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/Projects/{slug}/showcase", async (
                [FromRoute] string slug,
                [FromBody] ReplaceShowcaseRequest request,
                [FromServices] ICommandHandler<ReplaceShowcaseCommand> handler,
                CancellationToken ct) =>
            {
                var dto = new ShowcaseDto(request.ServiceName, request.EndpointUrl, request.HealthUrl);
                var cmd = new ReplaceShowcaseCommand(slug, dto);
                var result = await handler.Handle(cmd, ct);
                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .WithTags(Tags.Projects)
            .RequireAuthorization("SuperAdminOnly");
    }
}