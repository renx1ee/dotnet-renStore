using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Persistence.EntityTypeConfigurations;

internal sealed class DeliveryTrackingConfiguration
    : IEntityTypeConfiguration<DeliveryTrackingReadModel>
{
    public void Configure(EntityTypeBuilder<DeliveryTrackingReadModel> builder)
    {
        builder.ToTable("delivery_trackings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("tracking_id")
            .IsRequired();

        builder.Property(x => x.DeliveryOrderId)
            .HasColumnName("delivery_order_id")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(64)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<DeliveryStatus>(v))
            .IsRequired();

        builder.Property(x => x.CurrentLocation)
            .HasColumnName("current_location")
            .HasMaxLength(500);

        builder.Property(x => x.Notes)
            .HasColumnName("notes")
            .HasMaxLength(1000);

        builder.Property(x => x.SortingCenterId)
            .HasColumnName("sorting_center_id");

        builder.Property(x => x.PickupPointId)
            .HasColumnName("pickup_point_id");

        builder.Property(x => x.OccurredAt)
            .HasColumnName("occurred_at")
            .IsRequired();

        builder.HasIndex(x => x.DeliveryOrderId)
            .HasDatabaseName("ix_delivery_trackings_delivery_order_id");
    }
}