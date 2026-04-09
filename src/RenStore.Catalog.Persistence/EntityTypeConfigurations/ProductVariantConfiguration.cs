using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
            .Property(x => x.UpdatedById)
            .HasColumnName("updated_by_id")
            .IsRequired(false);
            
        builder
            .Property(x => x.UpdatedByRole)
            .HasColumnName("updated_by_role")
            .HasMaxLength(20)
            .IsRequired(false);
        
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
            .Property(v => v.ProductId)
            .HasColumnName("product_id");

        builder
            .Property(v => v.ColorId)
            .HasColumnName("color_id");
        
        // denormalization fields

        builder
            .Property(x => x.DiscountPercents)
            .HasColumnName("discount_percents")
            .HasMaxLength(3)
            .IsRequired(false);
        
        builder
            .Property(x => x.SellerIsVerified)
            .HasColumnName("is_verified_seller")
            .IsRequired(false);
        
        builder
            .Property(x => x.InStock)
            .HasColumnName("in_stock")
            .IsRequired(false);
        
        builder
            .Property(x => x.ReviewsCount)
            .HasColumnName("reviews_count")
            .IsRequired(false);
        
        builder
            .Property(x => x.AverageRating)
            .HasColumnName("average_rating")
            .IsRequired(false);
        
        builder
            .Property(x => x.SalesCount)
            .HasColumnName("sales_count")
            .IsRequired(false);
        
        builder
            .HasIndex(v => v.Article)
            .HasDatabaseName("idx_variant_article")
            .IsUnique(); 
        
        builder
            .HasIndex(v => v.Url)
            .HasDatabaseName("idx_variant_url")
            .IsUnique();
        
        // TODO: GIN gin_trgm_ops
        builder
            .HasIndex(v => v.NormalizedName)
            .HasDatabaseName("idx_variant_normalized_name");
        
        builder
            .HasIndex(v => v.SalesCount)
            .HasDatabaseName("idx_sales_count")
            .IsDescending();
        
        builder
            .HasIndex(v => v.AverageRating)
            .HasDatabaseName("idx_average_rating")
            .IsDescending();
        
        builder
            .HasIndex(v => v.ColorId)
            .HasDatabaseName("idx_color_id");
        
        // TODO: сделать индекс на
        // CREATE INDEX idx_products_published
        // ON products (id)
        // WHERE status='published';
        // 
        // CREATE INDEX idx_variants_published
        // ON product_variants (id)
        // WHERE status='published';
        // 
        // В ПРОДУКТ:
        // CREATE INDEX idx_products_category
        // ON products (category_id);
        // 
        // CREATE INDEX idx_products_subcategory
        // ON products (sub_category_id);
    }
}