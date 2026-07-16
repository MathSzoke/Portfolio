using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Curriculum;

namespace Portfolio.Infrastructure.Database.Configurations;

internal sealed class CurriculumAssetConfig : IEntityTypeConfiguration<CurriculumAsset>
{
    public void Configure(EntityTypeBuilder<CurriculumAsset> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Language).HasMaxLength(16).IsRequired();
        b.Property(x => x.Url).HasMaxLength(2048).IsRequired();
        b.HasIndex(x => x.Language).IsUnique();
        b.HasQueryFilter(x => !x.IsDeleted);
    }
}
