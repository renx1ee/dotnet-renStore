using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.Catalog.Persistence.EntityTypeConfigurations.Converters;
using RenStore.Catalog.Persistence.EntityTypeConfigurations.StatusConversions;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public sealed class VariantDetailConfiguration 
    : IEntityTypeConfiguration<VariantDetailReadModel>
{
    public void Configure(EntityTypeBuilder<VariantDetailReadModel> builder)
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
            .HasConversion<TypeOfPackingConverter>()
            .HasMaxLength(50)
            .IsRequired(false);
        
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
            .Property(x => x.CreatedAt)
            .HasColumnName("created_date")
            .IsRequired();
        
        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("updated_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.CountryOfManufactureId)
            .HasColumnName("country_id")
            .IsRequired();
        
        builder
            .Property(x => x.VariantId)
            .HasColumnName("variant_id")
            .IsRequired();
    }
}