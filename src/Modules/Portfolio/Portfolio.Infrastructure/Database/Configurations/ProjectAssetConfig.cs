using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Projects;

namespace Portfolio.Infrastructure.Database.Configurations;

internal sealed class ProjectAssetConfig : IEntityTypeConfiguration<ProjectAsset>
{
    public void Configure(EntityTypeBuilder<ProjectAsset> b)
    {
        b.HasKey(x => x.Id);

        b.Property(x => x.Kind).HasConversion<string>().IsRequired();
        b.Property(x => x.Url).IsRequired();
        b.Property(x => x.SortOrder).HasDefaultValue(0);

        b.HasOne(x => x.Project)
            .WithMany(x => x.Assets)
            .HasForeignKey(x => x.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        b.HasQueryFilter(x => !x.IsDeleted);
    }
}