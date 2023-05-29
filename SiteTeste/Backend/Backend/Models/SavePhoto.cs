using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public class SavePhoto
{
    [Key]
    public int PhotoID { get; set; }
    [ForeignKey("UserID")]
    public int UserID { get; set; }
    [Required]
    public string UrlPhoto { get; set; } = null!;
    [Required]
    public bool IsActive { get; set; }
}
