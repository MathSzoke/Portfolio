using System.ComponentModel.DataAnnotations;

namespace Backend.Data.SaveImageData.DTO;

public class UpdateSaveImageDTO
{
    [Required]
    public string UrlPhoto { get; set; } = null!;

    [Required]
    public bool IsActive { get; set; }
}
