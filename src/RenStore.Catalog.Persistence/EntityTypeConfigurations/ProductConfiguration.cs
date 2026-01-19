/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder
            .ToTable("products");
        
        builder
            .HasKey(p => p.Id);
        
        builder
            .Property(p => p.Id)
            .HasColumnName("product_id");
        
        builder
            .Property(p => p.IsBlocked)
            .HasColumnName("is_blocked")
            .HasDefaultValue(false)
            .IsRequired();
        
        builder
            .Property(p => p.OverallRating)
            .HasColumnName("overall_rating")
            .IsRequired()
            .HasDefaultValue(0);
        
        /*builder
            .HasOne(p => p.Seller)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SellerId)
            .HasConstraintName("seller_id");#1#

        builder
            .Property(p => p.SellerId)
            .HasColumnName("seller_id");
        
        builder
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .HasConstraintName("category_id");
        
        builder
            .Property(p => p.CategoryId)
            .HasColumnName("category_id");

        builder
            .HasMany(p => p.ProductVariants)
            .WithOne(v => v.Product)
            .HasForeignKey(x => x.ProductId);

        builder
            .HasOne(p => p.ProductCloth)
            .WithOne(c => c.Product)
            .HasForeignKey<ProductClothEntity>(p => p.ProductId);
    }
}*/