using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.User.Get;
using Portfolio.Domain.Users;
using SharedKernel;

namespace Portfolio.Application.User.GetByEmail;

internal sealed class GetUserByEmailQueryHandler(IApplicationDbContext db)
    : IQueryHandler<GetUserByEmailQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByEmailQuery q, CancellationToken ct)
    {
        var u = await db.Users
            .Include(x => x.ExternalLogins)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Email == q.email, ct);

        if (u is null) Result.Failure<UserResponse>(UserErrors.NotFoundByEmail(q.email));
        
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