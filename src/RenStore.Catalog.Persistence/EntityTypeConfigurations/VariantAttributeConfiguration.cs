using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public sealed class VariantAttributeConfiguration 
    : IEntityTypeConfiguration<VariantAttributeReadModel>
{
    public void Configure(EntityTypeBuilder<VariantAttributeReadModel> builder)
    {
        builder
            .ToTable("variant_attributes");
        
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id");

        builder
            .OwnsOne(x => x.Key, key =>
            {
                key
                    .Property(x => x.Key)
                    .HasColumnName("key")
                    .IsRequired();
                
                key
                    .HasIndex(x => x.Key)
                    .HasDatabaseName("ux_variant_attributes_key");
            });
            
        builder
            .OwnsOne(x => x.Value, value =>
            {
                value
                    .Property(x => x.Value)
                    .HasColumnName("value")
                    .IsRequired();
            });
        
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

        builder
            .Property(x => x.VariantId)
            .HasColumnName("variant_id");
        
        builder
            .HasIndex(x => x.VariantId)
            .HasDatabaseName("ux_variant_attributes_variant_id");
    }
}