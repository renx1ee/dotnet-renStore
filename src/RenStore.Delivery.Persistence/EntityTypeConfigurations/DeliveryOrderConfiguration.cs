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
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_date")
            .IsRequired();
        
        builder
            .Property(x => x.DeliveredAt)
            .HasColumnName("delivered_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.DeletedAt)
            .HasColumnName("deleted_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired();
        
        builder
            .Property(x => x.CurrentSortingCenterId)
            .HasColumnName("current_sorting_center_id")
            .IsRequired(false);
        
        builder
            .Property(x => x.DestinationSortingCenterId)
            .HasColumnName("destination_sorting_center_id")
            .IsRequired(false);
        
        builder
            .Property(x => x.PickupPointId)
            .HasColumnName("pickup_point_id")
            .IsRequired(false);
        
        builder
            .Property(x => x.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        builder
            .Property(x => x.AddressId)
            .HasColumnName("address_id")
            .IsRequired();
        
        builder
            .Property(x => x.DeliveryTariffId)
            .HasColumnName("delivery_tariff_id")
            .IsRequired();

        /*builder
            .HasOne(x => x.Order)
            .WithOne(x => x.DeliveryOrder)
            .HasForeignKey<DeliveryOrder>(x => x.OrderId);
        
        builder
            .HasOne(x => x.Address)
            .WithMany(x => x.Deliveries)
            .HasForeignKey(x => x.AddressId);
        
        builder
            .HasOne(x => x.DeliveryTariff)
            .WithMany(x => x.DeliveryOrders)
            .HasForeignKey(x => x.DeliveryTariffId);*/
    }
}