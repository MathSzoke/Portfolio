using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Chats;

namespace Portfolio.Infrastructure.Database.Configurations;

internal sealed class EmailOutboxConfig : IEntityTypeConfiguration<EmailOutbox>
{
    public void Configure(EntityTypeBuilder<EmailOutbox> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Status).HasConversion<string>().IsRequired();

        b.HasOne(x => x.Session)
            .WithMany()
            .HasForeignKey(x => x.SessionId)
            .OnDelete(DeleteBehavior.SetNull);

        b.HasIndex(x => new { x.Status, x.CreatedAt });
        
        b.HasQueryFilter(e => !e.IsDeleted);
    }
}