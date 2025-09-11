using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Chats;

namespace Portfolio.Infrastructure.Database.Configurations;

internal sealed class ChatMessageConfig : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Sender).HasConversion<string>().IsRequired();
        b.Property(x => x.Content).IsRequired();

        b.HasIndex(x => new { x.SessionId, x.CreatedAt });
        
        b.HasQueryFilter(c => !c.IsDeleted);
    }
}