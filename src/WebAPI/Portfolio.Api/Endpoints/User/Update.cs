using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.User;
using Portfolio.Api.Extensions;
using Portfolio.Api.Infrastructure;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.User.Get;
using Portfolio.Application.User.Update;

namespace Portfolio.Api.Endpoints.User;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/User", async (
                [FromBody] UpdateUserRequest request,
                [FromServices] ICommandHandler<UpdateUserCommand, UserResponse> handler,
                CancellationToken ct) =>
            {
                var cmd = new UpdateUserCommand(request.FullName, request.Email, request.ImageUrl, request.NewPassword);
                var result = await handler.Handle(cmd, ct);
                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .WithTags(Tags.User);
    }
}
