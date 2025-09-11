using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Projects;

namespace Portfolio.Infrastructure.Database.Configurations;

internal sealed class ProjectConfig : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Slug).IsRequired();
        b.HasIndex(x => x.Slug).IsUnique();

        b.Property(x => x.Name).IsRequired();
        b.Property(x => x.Summary).IsRequired();

        b.Property(x => x.Source).HasConversion<string>().IsRequired();
        b.Property(x => x.IsFeatured).HasDefaultValue(false);
        b.Property(x => x.SortOrder).HasDefaultValue(0);

        b.HasOne(x => x.Showcase)
            .WithOne(x => x.Project)
            .HasForeignKey<Showcase>(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        b.HasQueryFilter(s => !s.IsDeleted);
    }
}