using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Aggregates.VariantDetails;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class VariantDetailConfiguration 
    : IEntityTypeConfiguration<VariantDetail>
{
    public void Configure(EntityTypeBuilder<VariantDetail> builder)
    {
        builder
            .ToTable("variant_details");
        
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id");
        
        builder
            .Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(500)
            .IsRequired();
        
        builder
            .Property(x => x.Composition)
            .HasColumnName("composition")
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
            .Property(x => x.CaringOfThings)
            .HasColumnName("caring_of_things")
            .HasMaxLength(500)
            .IsRequired(false);

        builder
            .Property(x => x.TypeOfPacking) // TODO: dictionary to database
            .HasColumnName("type_of_packing")
            .IsRequired(false);
        
        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_date")
            .IsRequired();
        
        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("updated_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.Version)
            .HasColumnName("version")
            .IsRequired();
        
        builder
            .Property(x => x.CountryOfManufactureId)
            .HasColumnName("country_id")
            .IsRequired();
        
        builder
            .Property(x => x.VariantId)
            .HasColumnName("variant_id")
            .IsRequired();

        /*builder
            .HasOne(x => x.ProductVariant)
            .WithOne(x => x.ProductDetails)
            .HasForeignKey<ProductDetail>(x => x.ProductVariantId)
            .HasConstraintName("product_variant_id");*/
    }
}