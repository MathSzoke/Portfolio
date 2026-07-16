using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.User.Get;
using Portfolio.Application.User.GetById;

namespace Portfolio.Api.Endpoints.User;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/User/ById/{userId}", async (
                [FromRoute] Guid userId,
                [FromServices] IQueryHandler<GetUserByIdQuery, UserResponse> handler,
                CancellationToken ct) =>
        {
            var query = new GetUserByIdQuery(userId);
            var result = await handler.Handle(query, ct);
            return result.Match(
                Results.Ok,
                CustomResults.Problem);
        })
            .WithTags(Tags.User)
            .RequireAuthorization();
    }
}
