using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.Projects;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Projects.Add;
using Portfolio.Application.Projects.Reorder;

namespace Portfolio.Api.Endpoints.Projects;

internal sealed class Reorder : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/Projects/reorder", async (
                [FromBody] ReorderProjectsRequest request,
                [FromServices] ICommandHandler<ReorderProjectsCommand, ReorderResponse> handler,
                CancellationToken ct) =>
        {
            var items = request.Items.Select(i => new Item(i.Id, i.SortOrder)).ToList();
            var cmd = new ReorderProjectsCommand(items);
            var result = await handler.Handle(cmd, ct);
            return result.Match(
                id => Results.Ok(new { Id = id }),
                CustomResults.Problem);
        })
            .WithTags(Tags.Projects)
            .RequireAuthorization("SuperAdminOnly");
    }
}
