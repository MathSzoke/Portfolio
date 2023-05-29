using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class UserLogin
{
    [Key]
    public int UserID { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Error limit lenght")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(25, ErrorMessage = "Error limit lenght")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
