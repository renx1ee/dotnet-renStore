using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Aggregates.Variant;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class ProductVariantConfiguration 
    : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
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
            .Property(x => x.Version)
            .HasColumnName("version")
            .IsRequired();

        builder
            .Property(x => x.MainImageId)
            .HasColumnName("main_image_id");
        
        builder
            .Property(x => x.SizeSystem)
            .HasColumnName("size_system")
            .IsRequired();
        
        builder
            .Property(x => x.SizeType)
            .HasColumnName("size_type")
            .IsRequired();

        builder
            .HasMany(x => x.Sizes)
            .WithOne()
            .HasForeignKey(x => x.VariantId);
        
        builder
            .HasIndex(v => v.Article)
            .IsUnique();
        
        builder
            .HasIndex(v => v.NormalizedName)
            .IsUnique();
    }
}