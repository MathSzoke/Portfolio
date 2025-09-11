using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Domain.Users;
using SharedKernel;

namespace Portfolio.Application.User.Delete;

internal sealed class DeleteUserCommandHandler(IApplicationDbContext db, ICurrentUserContext userContext)
    : ICommandHandler<DeleteUserCommand, Result>
{
    public async Task<Result<Result>> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        var userId = userContext.UserIdGuid;

        var exists = await db.Users.IgnoreQueryFilters().AnyAsync(x => x.Id == userId, ct);
        if (!exists) return Result.Failure(UserErrors.NotFound(userId));

        await db.RefreshTokens.Where(x => x.UserId == userId).ExecuteDeleteAsync(ct);
        await db.ExternalLogins.Where(x => x.UserId == userId).ExecuteDeleteAsync(ct);

        var affected = await db.Users.IgnoreQueryFilters().Where(x => x.Id == userId).ExecuteDeleteAsync(ct);
        return affected == 0 ? Result.Failure(UserErrors.NotFound(userId)) : Result.Success();
    }
}