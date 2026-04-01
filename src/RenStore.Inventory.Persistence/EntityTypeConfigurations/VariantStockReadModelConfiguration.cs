using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Inventory.Domain.ReadModels;

namespace RenStore.Inventory.Persistence.EntityTypeConfigurations;

internal sealed class VariantStockReadModelConfiguration
    : IEntityTypeConfiguration<VariantStockReadModel>
{
    public void Configure(EntityTypeBuilder<VariantStockReadModel> builder)
    {
        builder
            .ToTable("stocks");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder
            .Property(x => x.InStock)
            .HasColumnName("in_stock")
            .HasDefaultValue(0)
            .IsRequired();
        
        builder
            .Property(x => x.Sales)
            .HasColumnName("sales")
            .HasDefaultValue(0)
            .IsRequired();

        builder
            .Property(x => x.WriteOffReason)
            .HasColumnName("write_off_reason")
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
            .Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
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
            .Property(v => v.VariantId)
            .HasColumnName("variant_id");
        
        builder
            .Property(v => v.SizeId)
            .HasColumnName("size_id");
    }
}