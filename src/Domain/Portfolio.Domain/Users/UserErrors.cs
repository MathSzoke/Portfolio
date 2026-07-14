using SharedKernel;

namespace Portfolio.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) =>
        Error.NotFound("Users.NotFound", $"The user with the Id = '{userId}' was not found");

    public static Error NotFoundByEmail(string email) =>
        Error.NotFound("Users.NotFoundByEmail", $"The user with the Email = '{email}' was not found");

    public static Error EmailInUse(string email) =>
        Error.Conflict("Users.EmailInUse", $"The email '{email}' is already in use");

    public static readonly Error InvalidCredentials =
        Error.Problem("Users.InvalidCredentials", "Invalid credentials");

    public static Error ExternalLoginNotFound(string provider, string subject) =>
        Error.NotFound("Users.ExternalLoginNotFound", $"External login '{provider}' with subject '{subject}' was not found");

    public static Error ExternalLoginAlreadyLinked(string provider, string subject) =>
        Error.Conflict("Users.ExternalLoginAlreadyLinked", $"External login '{provider}' with subject '{subject}' is already linked");

    public static readonly Error ProviderInvalid =
        Error.Failure("Users.ProviderInvalid", "The external provider is invalid");

    public static readonly Error FullNameRequired =
        Error.Failure("Users.FullNameRequired", "The full name is required");

    public static readonly Error EmailRequired =
        Error.Failure("Users.EmailRequired", "The email is required");

    public static readonly Error PasswordRequired =
        Error.Failure("Users.PasswordRequired", "The password is required");

    public static readonly Error Unauthorized =
        Error.Failure("Users.Unauthorized", "You are not authorized to perform this action.");

    public static readonly Error Forbidden =
        Error.Failure("Users.Forbidden", "You do not have permission to perform this action.");

    public static readonly Error ConcurrencyConflict =
        Error.Conflict("Users.ConcurrencyConflict", "The resource was modified by another process.");
}
