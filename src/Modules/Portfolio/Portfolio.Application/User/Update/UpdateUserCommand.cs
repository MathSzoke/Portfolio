using Portfolio.Application.Abstractions.Messaging;
using Portfolio.Application.User.Get;

namespace Portfolio.Application.User.Update;

public sealed record UpdateUserCommand(
    string FullName,
    string Email,
    string ImageUrl,
    string? NewPassword
) : ICommand<UserResponse>;