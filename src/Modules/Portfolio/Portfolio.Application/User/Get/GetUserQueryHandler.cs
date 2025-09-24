using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using SharedKernel;

namespace Portfolio.Application.User.Get;

internal sealed class GetUserQueryHandler(IApplicationDbContext db, ICurrentUserContext current)
    : IQueryHandler<GetUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery q, CancellationToken ct)
    {
        var userId = current.UserIdGuid;
        var u = await db.Users
            .Include(x => x.ExternalLogins)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == userId, ct);

        if(u is null) return Result.Failure<UserResponse>(Error.NotFound("User.NotFound", $"User '{userId}' not found."));
        
        var socialPhotos = u!.ExternalLogins
            .Select(x => new ExternalLoginPhoto(x.Provider, x.UserPhotoUrl))
            .ToList();

        var uploadedPhotos = await db.UserPhotos
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => x.PhotoUrl)
            .ToListAsync(ct);
        
        return Result.Success(new UserResponse(u.Id, u.FullName, u.Email, u.ImageUrl, socialPhotos, uploadedPhotos));
    }
}