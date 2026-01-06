using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Persistence.EntityTypeConfigurations;

public class DeliveryOrderConfiguration : IEntityTypeConfiguration<DeliveryOrder>
{
    public void Configure(EntityTypeBuilder<DeliveryOrder> builder)
    {
        builder
            .ToTable("delivery_orders");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("delivery_order_id")
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_date")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
            .IsRequired();
        
        builder
            .Property(x => x.DeliveredAt)
            .HasColumnName("delivered_date")
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);
        
        builder
            .Property(x => x.DeletedAt)
            .HasColumnName("deleted_date")
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);
        
        builder
            .Property(x => x.Status)
            .HasConversion<string>()
            .HasColumnName("status")
            .HasColumnType("varchar(50)")
            .IsRequired();
        
        builder
            .Property(x => x.CurrentSortingCenterId)
            .HasColumnName("current_sorting_center_id")
            .HasColumnType("bigint")
            .IsRequired(false);
        
        builder
            .Property(x => x.DestinationSortingCenterId)
            .HasColumnName("destination_sorting_center_id")
            .HasColumnType("bigint")
            .IsRequired(false);
        
        builder
            .Property(x => x.PickupPointId)
            .HasColumnName("pickup_point_id")
            .HasColumnType("bigint")
            .IsRequired(false);
        
        builder
            .Property(x => x.OrderId)
            .HasColumnName("order_id")
            .HasColumnType("uuid")
            .IsRequired();
        
        builder
            .Property(x => x.DeliveryTariffId)
            .HasColumnName("delivery_tariff_id")
            .HasColumnType("int")
            .IsRequired();
        
        builder
            .HasOne<DeliveryTariff>("_tariff")
            .WithMany(x => x.DeliveryOrders) 
            .HasForeignKey(x => x.DeliveryTariffId)
            .IsRequired(false);

        builder
            .HasIndex(x => x.CurrentSortingCenterId)
            .HasMethod("btree")
            .HasDatabaseName("idx_delivery_order_current_sorting_center_id_btree");
        
        builder
            .HasIndex(x => x.DestinationSortingCenterId)
            .HasMethod("btree")
            .HasDatabaseName("idx_delivery_order_destination_sorting_center_id_btree");
        
        builder
            .HasIndex(x => x.PickupPointId)
            .HasMethod("btree")
            .HasDatabaseName("idx_delivery_order_pickup_point_id_btree");
        
        builder
            .HasIndex(x => x.OrderId)
            .HasMethod("btree")
            .HasDatabaseName("idx_delivery_order_order_id_btree");
    }
}