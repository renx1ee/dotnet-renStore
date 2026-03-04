using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.Catalog.Persistence.EntityTypeConfigurations.StatusConversions;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public sealed class ProductVariantConfiguration 
    : IEntityTypeConfiguration<ProductVariantReadModel>
{
    public void Configure(EntityTypeBuilder<ProductVariantReadModel> builder)
    {
        builder
            .ToTable("product_variants");

        builder
            .HasKey(v => v.Id);

        builder
            .Property(v => v.Id)
            .HasColumnName("id");

        builder
            .Property(v => v.Name)
            .HasMaxLength(500)
            .HasColumnName("name")
            .IsRequired();

        builder
            .Property(v => v.NormalizedName)
            .HasColumnName("normalized_name")
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(v => v.Article)
            .HasColumnName("article")
            .IsRequired();

        builder
            .Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion(
                v => ProductVariantConversion.StatusToDatabase(v),
                v => ProductVariantConversion.StatusFromDatabase(v))
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .Property(v => v.Url)
            .HasColumnName("url")
            .HasMaxLength(500)
            .IsRequired();
        
        builder
            .Property(v => v.CreatedAt)
            .HasColumnName("created_date")
            .IsRequired();
        
        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("updated_date")
            .IsRequired(false);
            
        builder
            .Property(x => x.DeletedAt)
            .HasColumnName("deleted_date")
            .IsRequired(false);
        
        builder
            .Property(v => v.ProductId)
            .HasColumnName("product_id");

        builder
            .Property(v => v.ColorId)
            .HasColumnName("color_id");

        builder
            .Property(x => x.MainImageId)
            .HasColumnName("main_image_id");
        
        builder
            .Property(x => x.SizeSystem)
            .HasColumnName("size_system")
            .HasConversion(
                v => ProductVariantConversion.SizeSystemToDatabase(v),
                v => ProductVariantConversion.SizeSystemFromDatabase(v))
            .HasMaxLength(2)
            .IsRequired();
        
        builder
            .Property(x => x.SizeType)
            .HasColumnName("size_type")
            .HasConversion(
                v => ProductVariantConversion.SizeTypeToDatabase(v),
                v => ProductVariantConversion.SizeTypeFromDatabase(v))
            .HasMaxLength(50)
            .IsRequired();
        
        builder
            .HasIndex(v => v.Article)
            .IsUnique();
        
        builder
            .HasIndex(v => v.NormalizedName)
            .IsUnique();
    }
}