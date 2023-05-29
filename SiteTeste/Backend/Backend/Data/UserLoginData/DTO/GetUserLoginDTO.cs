namespace Backend.Data.UserLoginData.DTO;

public class GetUserLoginDTO
{
    public bool IsActived { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
