using System.ComponentModel.DataAnnotations;
using SharedKernel;

namespace Portfolio.Domain.Users;

public class UserPhotos : Entity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    [MaxLength(999)]
    public string PhotoUrl { get; set; } = null!;

    public User User { get; set; } = null!;
}