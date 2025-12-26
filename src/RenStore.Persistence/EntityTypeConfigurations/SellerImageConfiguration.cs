using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class SellerImageConfiguration : IEntityTypeConfiguration<SellerImageEntity>
{
    public void Configure(EntityTypeBuilder<SellerImageEntity> builder)
    {
        builder
            .ToTable("seller_images");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("seller_image_id");

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
            .HasOne(x => x.Seller)
            .WithMany(x => x.SellerImages)
            .HasForeignKey(x => x.SellerId);

        builder
            .Property(x => x.SellerId)
            .HasColumnName("seller_id");
    }
}