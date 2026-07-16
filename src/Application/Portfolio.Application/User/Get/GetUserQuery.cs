using Portfolio.Application.Abstractions.Messaging;

namespace Portfolio.Application.User.Get;

public sealed record GetUserQuery : IQuery<UserResponse>;

public sealed record UserResponse(
    Guid Id,
    string? FullName,
    string? Email,
    string? AvatarUrl,
    IReadOnlyList<ExternalLoginPhoto> SocialPhotos,
    List<string>? UploadedPhotos = null
);

public sealed record ExternalLoginPhoto(
    string Provider,
    string? UserPhotoUrl
);
