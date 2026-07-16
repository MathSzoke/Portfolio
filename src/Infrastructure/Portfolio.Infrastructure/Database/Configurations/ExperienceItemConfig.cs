using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Experiences;

namespace Portfolio.Infrastructure.Database.Configurations;

internal sealed class ExperienceItemConfig : IEntityTypeConfiguration<ExperienceItem>
{
    public void Configure(EntityTypeBuilder<ExperienceItem> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Company).HasMaxLength(160).IsRequired();
        b.Property(x => x.LogoUrl).HasMaxLength(2048).IsRequired();
        b.Property(x => x.RolePtBr).HasMaxLength(180).IsRequired();
        b.Property(x => x.RoleEnUs).HasMaxLength(180).IsRequired();
        b.Property(x => x.PeriodPtBr).HasMaxLength(180).IsRequired();
        b.Property(x => x.PeriodEnUs).HasMaxLength(180).IsRequired();
        b.Property(x => x.LocationPtBr).HasMaxLength(220).IsRequired();
        b.Property(x => x.LocationEnUs).HasMaxLength(220).IsRequired();
        b.Property(x => x.DescriptionPtBr).HasColumnType("text[]").IsRequired();
        b.Property(x => x.DescriptionEnUs).HasColumnType("text[]").IsRequired();
        b.Property(x => x.Techs).HasColumnType("text[]").IsRequired();
        b.Property(x => x.StartDate).HasMaxLength(32);
        b.HasIndex(x => x.SortOrder);
        b.HasQueryFilter(x => !x.IsDeleted);
    }
}
