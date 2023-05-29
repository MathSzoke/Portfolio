namespace Backend.Data.UserRegistrationData.DTO;

public class GetUserRegistrationDTO
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime Birthday { get; set; }
    public string Password { get; set; } = null!;
}
