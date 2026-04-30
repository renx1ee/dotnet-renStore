using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Order.Application.Saga;

namespace RenStore.Order.Persistence.Saga.Configurations;

public sealed class PlaceOrderSagaStateMap : SagaClassMap<PlaceOrderSagaState>
{
    protected override void Configure(
        EntityTypeBuilder<PlaceOrderSagaState> entity,
        ModelBuilder model)
    {
        entity
            .ToTable("place_order_sagas", "saga");
        
        entity
            .Property(x => x.CorrelationId)
            .HasColumnName("correlation_id")
            .IsRequired();

        entity
            .Property(x => x.CurrentState)
            .HasColumnName("current_state")
            .HasMaxLength(64)
            .IsRequired();

        entity
            .Property(x => x.CustomerId)
            .HasColumnName("customer_id")
            .IsRequired();

        entity
            .Property(x => x.VariantId)
            .HasColumnName("variant_id")
            .IsRequired();

        entity
            .Property(x => x.SizeId)
            .HasColumnName("size_id")
            .IsRequired();

        entity
            .Property(x => x.Quantity)
            .HasColumnName("quantity")
            .IsRequired();

        entity
            .Property(x => x.PriceAmount)
            .HasColumnName("price_amount")
            .HasPrecision(18, 2);

        entity
            .Property(x => x.Currency)
            .HasColumnName("currency")
            .HasMaxLength(8);

        entity
            .Property(x => x.ProductNameSnapshot)
            .HasColumnName("product_name_snapshot")
            .HasMaxLength(256);

        entity
            .Property(x => x.ShippingAddress)
            .HasColumnName("shipping_address")
            .HasMaxLength(500);

        /*entity
            .Property(x => x.PriceReceived)
            .HasColumnName("price_received")
            .IsRequired();*/

        entity
            .Property(x => x.AddressReceived)
            .HasColumnName("address_received")
            .IsRequired();

        entity
            .Property(x => x.OrderId)
            .HasColumnName("order_id");

        entity
            .Property(x => x.FailureReason)
            .HasColumnName("failure_reason")
            .HasMaxLength(1000);

        /*entity
            .Property(x => x.TimeoutTokenId)
            .HasColumnName("timeout_token_id");*/
        
        entity
            .HasIndex(x => x.CurrentState)
            .HasDatabaseName("ix_place_order_sagas_current_state");

        entity.HasIndex(x => x.CustomerId)
            .HasDatabaseName("ix_place_order_sagas_customer_id");
    }
}