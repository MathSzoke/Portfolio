using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.SaveImageData;

public class SaveImageContext : DbContext
{
    public SaveImageContext(DbContextOptions<SaveImageContext> options)
        : base(options)
    {

    }

    public DbSet<SavePhoto> SavePhoto { get; set; } = default!;
}
