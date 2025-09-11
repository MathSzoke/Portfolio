using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Projects;

namespace Portfolio.Infrastructure.Database.Configurations;

internal sealed class ShowcaseConfig : IEntityTypeConfiguration<Showcase>
{
    public void Configure(EntityTypeBuilder<Showcase> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.ServiceName).IsRequired();
        b.HasIndex(x => x.ProjectId).IsUnique();
        
        b.HasQueryFilter(s => !s.IsDeleted);
    }
}