using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Aggregates.Attribute;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class ProductAttributeConfiguration 
    : IEntityTypeConfiguration<VariantAttribute>
{
    public void Configure(EntityTypeBuilder<VariantAttribute> builder)
    {
        builder
            .ToTable("product_attributes");
        
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("attribute_id");
        
        builder
            .Property(x => x.Key)
            .HasColumnName("attribute_name")
            .IsRequired();
        
        builder
            .Property(x => x.Value)
            .HasColumnName("attribute_value")
            .IsRequired();
        
        builder
            .Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .HasColumnType("boolean")
            .HasDefaultValue("false")
            .IsRequired();
        
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
            .HasOne<ProductVariant>()
            .WithMany(x => x.ProductAttributes)
            .HasForeignKey(x => x.ProductVariantId)
            .HasConstraintName("product_variant_id");*/

        builder
            .Property(x => x.ProductVariantId)
            .HasColumnName("product_variant_id");
    }
}