using System.ComponentModel.DataAnnotations;

namespace Backend.Data.ContactData.DTO;
public class PostContactDTO
{
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
}
