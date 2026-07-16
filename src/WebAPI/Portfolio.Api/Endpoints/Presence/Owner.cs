using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.Presence.OwnerOnline;

namespace Portfolio.Api.Endpoints.Presence;

internal sealed class Owner : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/presence/owner", async (
                [FromQuery] string email,
                [FromServices] IQueryHandler<OwnerOnlineQuery, bool> handler,
                CancellationToken ct) =>
        {
            var result = await handler.Handle(new OwnerOnlineQuery(email), ct);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
            .WithTags(Tags.Presence);
    }
}
