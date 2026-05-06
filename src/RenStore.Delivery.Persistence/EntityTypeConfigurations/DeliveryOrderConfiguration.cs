using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Persistence.EntityTypeConfigurations;

internal sealed class DeliveryOrderConfiguration
    : IEntityTypeConfiguration<DeliveryOrderReadModel>
{
    public void Configure(EntityTypeBuilder<DeliveryOrderReadModel> builder)
    {
        builder.ToTable("delivery_orders");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("delivery_order_id")
            .IsRequired();

        builder.Property(x => x.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(x => x.DeliveryTariffId)
            .HasColumnName("delivery_tariff_id")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(64)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<DeliveryStatus>(v))
            .IsRequired();

        builder.Property(x => x.CurrentSortingCenterId)
            .HasColumnName("current_sorting_center_id");

        builder.Property(x => x.DestinationSortingCenterId)
            .HasColumnName("destination_sorting_center_id");

        builder.Property(x => x.PickupPointId)
            .HasColumnName("pickup_point_id");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.DeliveredAt)
            .HasColumnName("delivered_at");

        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at");
        
        builder.Property(x => x.TrackingNumber)
            .HasColumnName("tracking_number")
            .HasMaxLength(64);

        builder.Property(x => x.CurrentLocation)
            .HasColumnName("current_location")
            .HasMaxLength(500);

        builder.Property(x => x.PickupPostcode)
            .HasColumnName("pickup_postcode")
            .HasMaxLength(6);

        builder.HasIndex(x => x.TrackingNumber)
            .HasDatabaseName("ix_delivery_orders_tracking_number");

        builder.HasIndex(x => x.OrderId)
            .HasDatabaseName("ix_delivery_orders_order_id");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("ix_delivery_orders_status");
    }
}