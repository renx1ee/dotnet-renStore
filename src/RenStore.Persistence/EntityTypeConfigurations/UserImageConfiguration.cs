using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class UserImageConfiguration : IEntityTypeConfiguration<UserImageEntity>
{
    public void Configure(EntityTypeBuilder<UserImageEntity> builder)
    {
        builder
            .ToTable("user_images");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("user_image_id");

        builder
            .Property(x => x.OriginalFileName)
            .HasColumnName("original_file_name")
            .IsRequired();

        builder
            .Property(x => x.StoragePath)
            .HasColumnName("storage_path")
            .IsRequired();

        builder
            .Property(x => x.FileSizeBytes)
            .HasColumnName("file_size_bites")
            .IsRequired();

        builder
            .Property(x => x.IsMain)
            .HasColumnName("is_main")
            .HasDefaultValue(false)
            .IsRequired();

        builder
            .Property(x => x.SortOrder)
            .HasColumnName("sort_order")
            .IsRequired();

        builder
            .Property(x => x.UploadedAt)
            .HasColumnName("uploaded_date")
            .HasDefaultValue(DateTime.UtcNow)
            .IsRequired();

        builder
            .Property(x => x.Weight)
            .HasColumnName("weight")
            .IsRequired();
        
        builder
            .Property(x => x.Height)
            .HasColumnName("height")
            .IsRequired();

        builder
            .Property(x => x.UserId)
            .HasColumnName("user_id");
    }
}