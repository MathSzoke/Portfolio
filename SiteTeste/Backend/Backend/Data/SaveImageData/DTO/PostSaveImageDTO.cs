using System.ComponentModel.DataAnnotations;

namespace Backend.Data.SaveImageData.DTO;

public class PostSaveImageDTO
{
    [Required]
    public int UserID { get; set; }
    [Required]
    public string UrlPhoto { get; set; } = null!;

    [Required]
    public bool IsActive { get; set; }
}
