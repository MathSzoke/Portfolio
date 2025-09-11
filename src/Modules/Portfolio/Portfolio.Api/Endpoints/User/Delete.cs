using Microsoft.AspNetCore.Mvc;
using Portfolio.Api.Contracts.Auth;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.User.Delete;
using SharedKernel;

namespace Portfolio.Api.Endpoints.User;

internal sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/User", async (
                [FromServices] ICommandHandler<DeleteUserCommand, Result> handler,
                IConfiguration cfg,
                HttpContext http,
                CancellationToken ct) =>
            {
                var result = await handler.Handle(new DeleteUserCommand(), ct);
                if (result.IsFailure)
                {
                    return Results.Problem(
                        title: result.Error.Code,
                        detail: result.Error.Description,
                        statusCode: StatusCodes.Status404NotFound
                    );
                }

                RefreshTokenContract.ClearRefreshCookie(http, cfg);
                return Results.NoContent();
            })
            .WithTags(Tags.User)
            .RequireAuthorization();
    }
}