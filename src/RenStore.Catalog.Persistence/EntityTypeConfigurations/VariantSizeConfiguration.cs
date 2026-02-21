using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.EntityTypeConfigurations;

public class VariantSizeConfiguration : IEntityTypeConfiguration<VariantSize>
{
    public void Configure(EntityTypeBuilder<VariantSize> builder)
    {
        builder
            .ToTable("variant_sizes");
        
        builder
            .HasKey(s => s.Id);

        builder
            .Property(s => s.Id)
            .HasColumnName("id");

        builder
            .OwnsOne(x => x.Size, size =>
            {
                size.
                    Property(s => s.LetterSize)
                    .HasColumnName("letter_size")
                    .IsRequired();
                
                size
                    .Property(s => s.Number)
                    .HasColumnName("size_number")
                    .IsRequired();
                    
                size
                    .Property(s => s.System)
                    .HasColumnName("size_system")
                    .IsRequired();
                
                size
                    .Property(s => s.Type)
                    .HasColumnName("type")
                    .IsRequired();
            });
        
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
            .HasMany(s => s.Prices)
            .WithOne()
            .HasForeignKey(s => s.SizeId);
    }
}