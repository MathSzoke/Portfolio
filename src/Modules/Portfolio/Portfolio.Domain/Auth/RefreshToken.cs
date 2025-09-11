namespace Portfolio.Domain.Auth;

public class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string TokenHash { get; set; } = "";
    public DateTime ExpiresAtUtc { get; set; }
    public DateTime MaxExpiresAtUtc { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? RevokedAtUtc { get; set; }
    public string? RevokedReason { get; set; }
    public Guid? RotatedFromId { get; set; }
    public string? Device { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}