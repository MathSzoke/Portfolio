using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Chats;

namespace Portfolio.Infrastructure.Database.Configurations;

internal sealed class ChatSessionConfig : IEntityTypeConfiguration<ChatSession>
{
    public void Configure(EntityTypeBuilder<ChatSession> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Status).HasConversion<string>().IsRequired();
        b.Property(x => x.ConsentEmail).HasDefaultValue(true);

        b.HasIndex(x => new { x.Status, x.UpdatedAt });
        b.HasIndex(x => x.CreatedAt);

        b.HasMany(x => x.Messages)
            .WithOne(x => x.Session)
            .HasForeignKey(x => x.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        b.HasQueryFilter(c => !c.IsDeleted);
    }
}