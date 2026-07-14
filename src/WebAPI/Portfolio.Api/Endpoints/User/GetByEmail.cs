using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.User.Get;
using Portfolio.Application.User.GetByEmail;

namespace Portfolio.Api.Endpoints.User;

internal sealed class GetByEmail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/User/{email}", async (
                [FromRoute] string email,
                [FromServices] IQueryHandler<GetUserByEmailQuery, UserResponse> handler,
                CancellationToken ct) =>
            {
                var query = new GetUserByEmailQuery(email);
                var result = await handler.Handle(query, ct);
                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .WithTags(Tags.User);
    }
}
