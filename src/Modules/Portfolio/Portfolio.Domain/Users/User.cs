using System.ComponentModel.DataAnnotations;
using Portfolio.Domain.Users.Enums;
using SharedKernel;

namespace Portfolio.Domain.Users;

public sealed class User : Entity
{
    public Guid Id { get; init; }
    [MaxLength(50)]
    public string? FullName { get; set; }
    [MaxLength(50)]
    public string? Email { get; set; }
    [MaxLength(350)]
    public string? ImageUrl { get; set; }
    [MaxLength(100)]
    public string? PasswordHash { get; set; }
    public List<string> Roles { get; init; } = [];
    public UserStatus Status { get; set; } = UserStatus.Offline;
    public ICollection<ExternalLogin> ExternalLogins { get; init; } = [];
    public ICollection<UserPhotos>? UserPhotos { get; init; } = [];
}
