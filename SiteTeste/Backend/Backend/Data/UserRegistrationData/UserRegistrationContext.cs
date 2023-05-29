using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.UserRegistrationData;

public class UserRegistrationContext : DbContext
{
    public UserRegistrationContext(DbContextOptions<UserRegistrationContext> options) : base(options) { }

    public DbSet<UserRegistration> UserRegister { get; set; } = default!;
}
