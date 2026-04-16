using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Order.Domain.ReadModels;
using RenStore.Order.Persistence.EntityTypeConfiguration.Conversions;

namespace RenStore.Order.Persistence.EntityTypeConfiguration;

internal sealed class OrderConfiguration
    : IEntityTypeConfiguration<OrderReadModel>
{
    public void Configure(EntityTypeBuilder<OrderReadModel> builder)
    {
        builder.ToTable("order_details");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(x => x.CustomerId)
            .HasColumnName("customer_id")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(
                v => OrderConversion.OrderStatusToDatabase(v),
                v => OrderConversion.OrderStatusFromDatabase(v));

        builder.Property(x => x.ShippingAddress)
            .HasColumnName("shipping_address")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.TrackingNumber)
            .HasColumnName("tracking_number")
            .HasMaxLength(100);

        builder.Property(x => x.CancellationReason)
            .HasColumnName("cancellation_reason")
            .HasMaxLength(1000);

        builder.Property(x => x.TotalAmount)
            .HasColumnName("total_amount")
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        builder.HasMany(x => x.Items)
            .WithOne()
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.CustomerId)
            .HasDatabaseName("ix_order_details_customer_id");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("ix_order_details_status");
    }
}