using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class ProductDetailConfiguration : IEntityTypeConfiguration<ProductDetail>
{
    public void Configure(EntityTypeBuilder<ProductDetail> builder)
    {
        builder
            .ToTable("product_details");
        
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("product_detail_id");
        
        builder
            .Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(500)
            .IsRequired();
        
        builder
            .Property(x => x.ModelFeatures)
            .HasColumnName("model_features")
            .HasMaxLength(500)
            .IsRequired(false);
        
        builder
            .Property(x => x.DecorativeElements)
            .HasColumnName("decorative_elements")
            .HasMaxLength(500)
            .IsRequired(false);
        
        builder
            .Property(x => x.Equipment)
            .HasColumnName("equipment")
            .HasMaxLength(500)
            .IsRequired(false);
        
        builder
            .Property(x => x.Composition)
            .HasColumnName("composition")
            .HasMaxLength(250)
            .IsRequired();
        
        builder
            .Property(x => x.CaringOfThings)
            .HasColumnName("caring_of_things")
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(x => x.TypeOfPacking)
            .HasColumnName("type_of_packing")
            .IsRequired(false);
        
        builder
            .Property(x => x.CountryOfManufactureId)
            .HasColumnName("country_id")
            .IsRequired();

        /*builder
            .HasOne(x => x.ProductVariant)
            .WithOne(x => x.ProductDetails)
            .HasForeignKey<ProductDetail>(x => x.ProductVariantId)
            .HasConstraintName("product_variant_id");*/

        builder
            .Property(x => x.ProductVariantId)
            .HasColumnName("product_variant_id");
    }
}