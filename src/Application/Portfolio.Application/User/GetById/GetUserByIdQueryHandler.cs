using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.User.Get;
using Portfolio.Domain.Users;
using SharedKernel;

namespace Portfolio.Application.User.GetById;

internal sealed class GetUserByIdQueryHandler(IApplicationDbContext db)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery q, CancellationToken ct)
    {
        var u = await db.Users
            .Include(x => x.ExternalLogins)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == q.UserId, ct);

        if (u is null) return Result.Failure<UserResponse>(UserErrors.NotFound(q.UserId));

        var socialPhotos = u!.ExternalLogins
            .Select(x => new ExternalLoginPhoto(x.Provider, x.UserPhotoUrl))
            .ToList();

        var uploadedPhotos = await db.UserPhotos
            .Where(x => x.UserId == u.Id)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.PhotoUrl)
            .ToListAsync(ct);

        return Result.Success(new UserResponse(u.Id, u.FullName, u.Email, u.ImageUrl, socialPhotos, uploadedPhotos));
    }
}