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
        b.HasIndex(x => new { x.SenderId, x.RecipientId, x.Status });
        b.HasIndex(x => x.RecipientId);
        b.HasIndex(x => x.SenderId);
        b.HasIndex(x => x.CreatedAt);
        b.HasIndex(x => x.UpdatedAt);
        b.HasMany(x => x.Messages)
            .WithOne(x => x.Session)
            .HasForeignKey(x => x.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
        b.HasQueryFilter(x => !x.IsDeleted);
    }
}
