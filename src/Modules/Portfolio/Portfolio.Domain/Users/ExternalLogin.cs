using SharedKernel;

namespace Portfolio.Domain.Users;

public sealed class ExternalLogin : Entity
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Provider { get; init; } = null!;
    public string ProviderUserId { get; init; } = null!;
    public string? UserPhotoUrl { get; set; }
    public User User { get; init; } = null!;
}
