using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Entities;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder
            .ToTable("product_variants");

        builder
            .HasKey(v => v.Id);

        builder
            .Property(v => v.Id)
            .HasColumnName("product_variant_id");

        builder
            .Property(v => v.Name)
            .HasMaxLength(500)
            .HasColumnName("variant_name")
            .IsRequired();

        builder
            .Property(v => v.NormalizedName)
            .HasColumnName("normalized_variant_name")
            .HasMaxLength(500)
            .IsRequired();

        builder
            .HasIndex(v => v.NormalizedName)
            .IsUnique();

        builder
            .Property(v => v.Rating)
            .HasColumnName("rating")
            .HasDefaultValue(0)
            .IsRequired();

        builder
            .Property(v => v.Article)
            .HasColumnName("article")
            .IsRequired();

        builder
            .HasIndex(v => v.Article)
            .IsUnique();
        
        builder
            .Property(v => v.InStock)
            .HasColumnName("in_stock")
            .HasDefaultValue(0)
            .IsRequired();
        
        builder
            .Property(v => v.IsAvailable)
            .HasColumnName("is_available")
            .IsRequired();
        
        builder
            .Property(v => v.CreatedAt)
            .HasColumnName("created_date")
            .HasDefaultValue(DateTime.UtcNow)
            .IsRequired();
        
        builder
            .Property(v => v.Url)
            .HasColumnName("url")
            .HasMaxLength(500)
            .IsRequired();

        /*builder
            .HasOne(v => v.Product)
            .WithMany(p => p.ProductVariantIds)
            .HasForeignKey(v => v.ProductId);*/

        builder
            .Property(v => v.ProductId)
            .HasColumnName("product_id");
        
        /*builder
            .HasOne(v => v.Color)
            .WithMany(c => c.ProductVariantIds)
            .HasForeignKey(v => v.ColorId);*/
        
        builder
            .Property(v => v.ColorId)
            .HasColumnName("color_id");

        /*builder
            .HasOne(v => v.ProductDetails)
            .WithOne(d => d.ProductVariant);*/
        
        /*builder
            .HasMany(v => v.ProductAttributes)
            .WithOne(a => a.ProductVariant)
            .HasForeignKey(a => a.ProductVariantId);*/
        
        builder
            .HasMany(v => v.PriceHistories)
            .WithOne(p => p.ProductVariant)
            .HasForeignKey(v => v.ProductVariantId);
    }
}