using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data.ContactData;

public class ContactContext : DbContext
{
    public ContactContext(DbContextOptions<ContactContext> options)
        : base(options)
    {

    }

    public DbSet<Contact> Contact { get; set; } = default!;
}
