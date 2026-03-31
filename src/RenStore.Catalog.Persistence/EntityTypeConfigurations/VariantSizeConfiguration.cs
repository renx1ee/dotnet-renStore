using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public sealed class VariantSizeConfiguration
    : IEntityTypeConfiguration<VariantSizeReadModel>
{
    public void Configure(EntityTypeBuilder<VariantSizeReadModel> builder)
    {
        builder
            .ToTable("variant_sizes");
        
        builder
            .HasKey(s => s.Id);

        builder
            .Property(s => s.Id)
            .HasColumnName("id");

        builder.
            Property(s => s.LetterSize)
            .HasColumnName("letter_size")
            .IsRequired();
                
        builder
            .Property(s => s.Number)
            .HasColumnName("size_number")
            .IsRequired(false);
                    
        builder
            .Property(s => s.System)
            .HasColumnName("size_system") 
            .IsRequired();

        builder
            .Property(s => s.Type)
            .HasColumnName("type")
            .IsRequired();
        
        builder
            .Property(x => x.UpdatedById)
            .HasColumnName("updated_by_id")
            .IsRequired();
            
        builder
            .Property(x => x.UpdatedByRole)
            .HasColumnName("updated_by_role")
            .HasMaxLength(20)
            .IsRequired();
        
        builder
            .Property(s => s.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
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
            .HasColumnName("variant_id")
            .IsRequired();

        builder
            .HasIndex(x => x.VariantId)
            .HasDatabaseName("ux_variant_sizes_variant_id");
    }
}