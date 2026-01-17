/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImageEntity>
{
    public void Configure(EntityTypeBuilder<ProductImageEntity> builder)
    {
        builder
            .ToTable("product_images");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("product_image_id");

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
            .HasOne(x => x.ProductVariant)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.ProductVariantId);

        builder
            .Property(x => x.ProductVariantId)
            .HasColumnName("product_variant_id");
    }
}*/