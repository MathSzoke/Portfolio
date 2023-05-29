using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public class Contact
{
    [Key]
    [Required]
    public int IDContact { get; set; }

    [Required(ErrorMessage = "Campo nome é obrigatório!")]
    [StringLength(100, ErrorMessage = "O limite máximo de caracteres para o campo de nome é 100.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Campo e-mail é obrigatório!")]
    [StringLength(100, ErrorMessage = "O limite máximo de caracteres para o campo de nome é 100.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Campo de assunto é obrigatório!")]
    [StringLength(100, ErrorMessage = "O limite máximo de caracteres para o campo de nome é 100.")]
    public string Subject { get; set; } = null!;

    [Required(ErrorMessage = "Campo de mensagem é obrigatório!")]
    [StringLength(100, ErrorMessage = "O limite máximo de caracteres para o campo de nome é 8000.")]
    public string Message { get; set; } = null!;
    public DateTime ContactHour { get; set; } = DateTime.UtcNow;
}
