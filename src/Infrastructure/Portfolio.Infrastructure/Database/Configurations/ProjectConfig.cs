using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Projects;

namespace Portfolio.Infrastructure.Database.Configurations;

internal sealed class ProjectConfig : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Name).IsRequired();
        b.Property(x => x.Summary).IsRequired();

        b.Property(x => x.ThumbnailUrl).IsRequired(false);
        b.Property(x => x.ProjectUrl).IsRequired(false);
        b.Property(x => x.RepoName).IsRequired(false);

        b.Property(x => x.Rating)
            .HasDefaultValue(0)
            .IsRequired();

        b.Property(x => x.RatingCount)
            .HasDefaultValue(0)
            .IsRequired();

        b.Property(x => x.SortOrder).HasDefaultValue(0);

        b.HasQueryFilter(s => !s.IsDeleted);
    }
}
