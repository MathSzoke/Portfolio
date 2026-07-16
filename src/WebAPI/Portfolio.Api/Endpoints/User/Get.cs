using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.User.Get;

namespace Portfolio.Api.Endpoints.User;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/User", async (
                [FromServices] IQueryHandler<GetUserQuery, UserResponse> handler,
                CancellationToken ct) =>
            {
                var query = new GetUserQuery();
                var result = await handler.Handle(query, ct);
                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .WithTags(Tags.User);
    }
}
