using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class DeliveryTrackingConfiguration : IEntityTypeConfiguration<DeliveryTrackingEntity>
{
    public void Configure(EntityTypeBuilder<DeliveryTrackingEntity> builder)
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
            .IsRequired();

        builder
            .Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_date")
            .HasDefaultValue(DateTime.UtcNow)
            .IsRequired();

        builder
            .Property(x => x.Notes)
            .HasColumnName("notes")
            .IsRequired(false);

        builder
            .Property(x => x.DeliveryOrderId)
            .HasColumnName("delivery_order_id")
            .IsRequired();

        builder
            .HasOne(x => x.DeliveryOrder)
            .WithMany(x => x.TrackingHistory)
            .HasForeignKey(x => x.DeliveryOrderId);
    }
}