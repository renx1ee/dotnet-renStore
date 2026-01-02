/*using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Delivery.Domain.Entities;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

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
            .IsRequired();
        
        builder
            .Property(x => x.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        /*builder
            .HasOne(x => x.Order)
            .WithOne(x => x.DeliveryOrder)
            .HasForeignKey<DeliveryOrder>(x => x.OrderId);#1#
        
        builder
            .Property(x => x.OrderId)
            .HasColumnName("address_id")
            .IsRequired();

        builder
            .HasOne(x => x.Address)
            .WithMany(x => x.Deliveries)
            .HasForeignKey(x => x.AddressId);
        
        builder
            .Property(x => x.DeliveryTariffId)
            .HasColumnName("delivery_tariff_id")
            .IsRequired();
        
        builder
            .HasOne(x => x.DeliveryTariff)
            .WithMany(x => x.DeliveryOrders)
            .HasForeignKey(x => x.DeliveryTariffId);
    }
}*/