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
            .HasColumnName("delivery_tracking_history_id")
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder
            .Property(x => x.CurrentLocation)
            .HasColumnName("current_location")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired(false);

        builder
            .Property(x => x.Status)
            .HasConversion<string>()
            .HasColumnName("status")
            .HasColumnType("varchar(50)")
            .IsRequired();

        builder
            .Property(x => x.Notes)
            .HasColumnName("notes")
            .HasColumnType("varchar(150)")
            .HasMaxLength(150)
            .IsRequired(false);
        
        builder
            .Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .HasColumnType("boolean")
            .HasDefaultValueSql("false")
            .IsRequired();
        
        builder
            .Property(x => x.OccurredAt)
            .HasColumnName("created_date")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
            .IsRequired();
        
        builder
            .Property(x => x.DeletedAt)
            .HasColumnName("deleted_date")
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);
        
        builder
            .Property(x => x.SortingCenterId)
            .HasColumnName("sorting_center_id")
            .HasColumnType("bigint")
            .IsRequired(false);
        
        builder
            .Property(x => x.PickupPointId)
            .HasColumnName("pickup_point_id")
            .HasColumnType("bigint")
            .IsRequired(false);
        
        builder
            .HasOne(typeof(SortingCenter), "_sortingCenter")
            .WithMany()
            .HasForeignKey("SortingCenterId")
            .IsRequired(false);
        
        builder
            .HasOne(typeof(PickupPoint), "_pickupPoint")
            .WithMany()
            .HasForeignKey("PickupPointId")
            .IsRequired(false);

        builder
            .Property(x => x.DeliveryOrderId)
            .HasColumnName("delivery_order_id")
            .IsRequired();
        
        builder
            .HasOne<DeliveryOrder>("_deliveryOrder")
            .WithMany(x => x.TrackingHistory)
            .HasForeignKey(x => x.DeliveryOrderId)
            .IsRequired();
    }
}