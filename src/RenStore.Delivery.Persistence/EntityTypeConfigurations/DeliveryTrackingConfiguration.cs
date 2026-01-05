using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Persistence.EntityTypeConfigurations;

public class DeliveryTrackingConfiguration : IEntityTypeConfiguration<DeliveryTracking>
{
    public void Configure(EntityTypeBuilder<DeliveryTracking> builder)
    {
        builder
            .ToTable("delivery_tracking_history");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("delivery_tracking_history_id");

        builder
            .Property(x => x.CurrentLocation)
            .HasColumnName("current_location")
            .IsRequired(false);

        builder
            .Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired();

        builder
            .Property(x => x.Notes)
            .HasColumnName("notes")
            .IsRequired(false);
        
        builder
            .Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired(false);
        
        builder
            .Property(x => x.OccurredAt)
            .HasColumnName("created_date")
            .IsRequired();
        
        builder
            .Property(x => x.DeletedAt)
            .HasColumnName("deleted_date")
            .IsRequired();
        
        builder
            .Property(x => x.SortingCenterId)
            .HasColumnName("sorting_center_id")
            .IsRequired(false);

        builder
            .Property(x => x.DeliveryOrderId)
            .HasColumnName("delivery_order_id")
            .IsRequired();

        /*builder
            .HasOne(x => x.DeliveryOrderId)
            .WithMany(x => x.TrackingHistory)
            .HasForeignKey(x => x.DeliveryOrderId);*/
    }
}