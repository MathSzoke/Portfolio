using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Projects;

namespace Portfolio.Infrastructure.Database.Configurations;

internal sealed class TagConfig : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).IsRequired();
        b.HasIndex(x => x.Name).IsUnique();
        
        b.HasQueryFilter(s => !s.IsDeleted);
    }
}