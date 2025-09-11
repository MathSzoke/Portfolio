namespace Portfolio.Api.Contracts.User;

public sealed record UpdateUserRequest(
    string ImageUrl,
    string Email,
    string FullName,
    string? NewPassword
);
