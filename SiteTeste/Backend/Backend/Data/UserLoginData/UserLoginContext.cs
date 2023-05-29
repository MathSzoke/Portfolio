using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.UserLoginData;

public class UserLoginContext : DbContext
{
    public UserLoginContext(DbContextOptions<UserLoginContext> options) : base(options) { }

    public DbSet<UserLogin> UserLogins { get; set; } = default!;
}
