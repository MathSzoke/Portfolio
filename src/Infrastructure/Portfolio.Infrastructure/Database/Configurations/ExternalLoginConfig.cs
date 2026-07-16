using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Users;

namespace Portfolio.Infrastructure.Database.Configurations;

internal sealed class ExternalLoginConfig : IEntityTypeConfiguration<ExternalLogin>
{
    public void Configure(EntityTypeBuilder<ExternalLogin> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Provider).IsRequired().HasMaxLength(64);
        b.Property(x => x.ProviderUserId).IsRequired().HasMaxLength(256);
        b.HasIndex(x => new { x.Provider, x.ProviderUserId }).IsUnique();
        b.HasIndex(x => x.UserId);
        b.HasOne(x => x.User)
            .WithMany(x => x.ExternalLogins)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}