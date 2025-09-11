using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Users;

namespace Portfolio.Infrastructure.Database.Configurations;

public class UserPhotoConfig : IEntityTypeConfiguration<UserPhotos>
{
    public void Configure(EntityTypeBuilder<UserPhotos> builder)
    {
        builder.ToTable("UserPhotos");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PhotoUrl)
            .IsRequired()
            .HasMaxLength(400);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(u => u.UserPhotos)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}