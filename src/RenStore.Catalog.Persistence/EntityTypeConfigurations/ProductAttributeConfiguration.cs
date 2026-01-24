/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class ProductAttributeConfiguration 
    : IEntityTypeConfiguration<ProductAttribute>
{
    public void Configure(EntityTypeBuilder<ProductAttribute> builder)
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
            .HasOne(x => x.ProductVariant)
            .WithMany(x => x.ProductAttributes)
            .HasForeignKey(x => x.ProductVariantId)
            .HasConstraintName("product_variant_id");

        builder
            .Property(x => x.ProductVariantId)
            .HasColumnName("product_variant_id");
    }
}*/