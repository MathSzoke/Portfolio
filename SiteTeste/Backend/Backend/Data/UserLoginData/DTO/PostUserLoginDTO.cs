using System.ComponentModel.DataAnnotations;

namespace Backend.Data.UserLoginData.DTO;

public class PostUserLoginDTO
{
    [Required(ErrorMessage = "Field Name is required!")]
    [StringLength(50, ErrorMessage = "Error limit lenght")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Field Password is required!")]
    [StringLength(25, ErrorMessage = "Error limit lenght")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
