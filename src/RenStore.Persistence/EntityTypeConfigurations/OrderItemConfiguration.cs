using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder
            .ToTable("order_items");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("order_item_id");

        builder
            .Property(x => x.Price)
            .HasColumnName("price")
            .IsRequired();
        
        builder
            .Property(x => x.TotalPrice)
            .HasColumnName("total_price")
            .IsRequired();
        
        builder
            .Property(x => x.Amount)
            .HasColumnName("amount")
            .HasDefaultValue(1)
            .IsRequired();
        
        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_date")
            .IsRequired();
        
        builder
            .Property(x => x.CancelledAt)
            .HasColumnName("cancelled_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired();
        
        builder
            .Property(x => x.ReturnReason)
            .HasColumnName("return_reason")
            .IsRequired(false);
        
        builder
            .Property(x => x.ReturnedAmount)
            .HasColumnName("returned_amount")
            .IsRequired(false);
        
        builder
            .Property(x => x.WarrantyStartDate)
            .HasColumnName("warranty_start_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.WarrantyEndDate)
            .HasColumnName("warranty_end_date")
            .IsRequired(false);

        builder
            .Property(x => x.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        /*builder
            .HasOne(x => x.Order)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.OrderId);*/
        
        builder
            .Property(x => x.ProductId)
            .HasColumnName("product_id")
            .IsRequired();

        /*builder
            .HasOne(x => x.Product)
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.ProductId);*/
    }
}