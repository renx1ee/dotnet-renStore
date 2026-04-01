using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Inventory.Domain.ReadModels;
using RenStore.Inventory.Persistence.EntityTypeConfigurations.Conversions;
using RenStore.Inventory.Persistence.EntityTypeConfigurations.Converters;

namespace RenStore.Inventory.Persistence.EntityTypeConfigurations;

internal sealed class VariantReservationReadModelConfiguration
    : IEntityTypeConfiguration<VariantReservationReadModel>
{
    public void Configure(EntityTypeBuilder<VariantReservationReadModel> builder)
    {
        builder
            .ToTable("reservations");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder
            .Property(x => x.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder
            .Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion(
                r => VariantReservationConversion.ReservationStatusToDatabase(r),
                r => VariantReservationConversion.ReservationStatusFromDatabase(r))
            .IsRequired();
        
        builder
            .Property(x => x.CancelReason)
            .HasColumnName("cancel_reason")
            .HasConversion<CancelReasonConverter>()
            .IsRequired(false);
        
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
            .Property(v => v.CreatedAt)
            .HasColumnName("created_date")
            .IsRequired();
        
        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("updated_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.ExpiresAt)
            .HasColumnName("expires_date")
            .IsRequired();
            
        builder
            .Property(x => x.DeletedAt)
            .HasColumnName("deleted_date")
            .IsRequired(false);
        
        builder
            .Property(v => v.VariantId)
            .HasColumnName("variant_id");
        
        builder
            .Property(v => v.SizeId)
            .HasColumnName("size_id");
        
        builder
            .Property(v => v.OrderId)
            .HasColumnName("order_id");
        
        // TODO: indexes
    }
}