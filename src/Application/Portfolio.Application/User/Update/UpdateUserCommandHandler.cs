using Microsoft.EntityFrameworkCore;
using Portfolio.Application.Abstractions.Authentication;
using Portfolio.Application.Abstractions.Data;
using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.User.Get;
using Portfolio.Domain.Users;
using SharedKernel;

namespace Portfolio.Application.User.Update;

internal sealed class UpdateUserCommandHandler(IApplicationDbContext db, ICurrentUserContext current, IPasswordHasher hasher)
    : ICommandHandler<UpdateUserCommand, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(UpdateUserCommand cmd, CancellationToken ct)
    {
        var userId = current.UserIdGuid;

        var u = await db.Users.FirstOrDefaultAsync(x => x.Id == userId, ct);
        if (u is null)
            return Result.Failure<UserResponse>(Error.Failure(UserErrors.NotFound(userId).Code, UserErrors.NotFound(userId).Description));
        
        if (!string.Equals(u.Email, cmd.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailInUse = await db.Users.AnyAsync(x => x.Email == cmd.Email && x.Id != userId, ct);
            if (emailInUse)
                return Result.Failure<UserResponse>(Error.Conflict(UserErrors.EmailInUse(cmd.Email).Code, UserErrors.EmailInUse(cmd.Email).Description));
            u.Email = cmd.Email;
        }
        
        u.FullName = cmd.FullName;
        u.ImageUrl = cmd.ImageUrl;
        if (!string.IsNullOrWhiteSpace(cmd.NewPassword))
            u.PasswordHash = hasher.Hash(cmd.NewPassword);

        if (!string.IsNullOrWhiteSpace(cmd.ImageUrl))
        {
            var already = await db.UserPhotos.AnyAsync(x => x.UserId == userId && x.PhotoUrl == cmd.ImageUrl, ct);
            if (!already)
            {
                db.UserPhotos.Add(new UserPhotos
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    PhotoUrl = cmd.ImageUrl
                });
            }
        }

        await db.SaveChangesAsync(ct);

        var socialPhotos = u.ExternalLogins
            .Where(x => !string.IsNullOrWhiteSpace(x.UserPhotoUrl))
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
