using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Aggregates.Product;
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
            .HasColumnName("id");
        
        builder
            .Property(p => p.Status)
            .HasColumnName("status")
            .IsRequired();
        
        /*builder
            .Property(p => p.OverallRating)
            .HasColumnName("overall_rating")
            .IsRequired()
            .HasDefaultValue(0);*/
        
        builder
            .Property(x => x.CreatedAt)
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
        
        /*builder
            .HasOne(p => p.Seller)
            .WithMany(s => s.Products)
            .HasForeignKey(p => p.SellerId)
            .HasConstraintName("seller_id");*/

        builder
            .Property(p => p.SellerId)
            .HasColumnName("seller_id")
            .IsRequired();
        
        builder
            .Property(p => p.SubCategoryId)
            .HasColumnName("sub_category_id")
            .IsRequired();
        
        /*builder
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .HasConstraintName("category_id");
    
        builder
            .HasMany(p => p.ProductVariantIds)
            .WithOne(v => v.Product)
            .HasForeignKey(x => x.ProductId);

        builder
            .HasOne(p => p.ProductCloth)
            .WithOne(c => c.Product)
            .HasForeignKey<ProductCloth>(p => p.ProductId);*/
    }
}