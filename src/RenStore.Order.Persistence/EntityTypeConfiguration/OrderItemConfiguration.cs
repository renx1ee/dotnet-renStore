using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Order.Domain.ReadModels;
using RenStore.Order.Persistence.EntityTypeConfiguration.Conversions;

namespace RenStore.Order.Persistence.EntityTypeConfiguration;

internal sealed class OrderItemProjectionConfiguration
    : IEntityTypeConfiguration<OrderItemProjection>
{
    public void Configure(EntityTypeBuilder<OrderItemProjection> builder)
    {
        builder.ToTable("order_items");

        builder.HasKey(x => x.OrderItemId);

        builder.Property(x => x.OrderItemId)
            .HasColumnName("order_item_id")
            .IsRequired();

        builder.Property(x => x.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        builder.Property(x => x.VariantId)
            .HasColumnName("variant_id")
            .IsRequired();

        builder.Property(x => x.SizeId)
            .HasColumnName("size_id")
            .IsRequired();

        builder.Property(x => x.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        builder.Property(x => x.PriceAmount)
            .HasColumnName("price_amount")
            .IsRequired()
            .HasPrecision(18, 2);

        // TODO:
        builder.Property(x => x.Currency)
            .HasColumnName("currency")
            .IsRequired()
            .HasMaxLength(10)
            .HasConversion(
                v => CurrencyConversion.CurrencyToDatabase(v),
                v => CurrencyConversion.CurrencyFromDatabase(v));

        builder.Property(x => x.ProductNameSnapshot)
            .HasColumnName("product_name_snapshot")
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasMaxLength(20)
            .HasConversion(
                v => OrderConversion.OrderItemStatusToDatabase(v),
                v => OrderConversion.OrderItemStatusFromDatabase(v));

        builder.Property(x => x.CancellationReason)
            .HasColumnName("cancellation_reason")
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        // TotalPrice — вычисляемое свойство, не маппится в БД
        builder.Ignore(x => x.TotalPrice);

        builder.HasIndex(x => x.OrderId)
            .HasDatabaseName("ix_order_item_projections_order_id");

        builder.HasIndex(x => x.VariantId)
            .HasDatabaseName("ix_order_item_projections_variant_id");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("ix_order_item_projections_status");
    }
}