using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder
            .ToTable("orders");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("order_id");
        

        builder
            .Property(x => x.TotalPrice)
            .HasColumnName("total_price")
            .IsRequired();
        
        builder
            .Property(x => x.SubTotalPrice)
            .HasColumnName("sub_total_price")
            .IsRequired();
        
        builder
            .Property(x => x.TaxAmount)
            .HasColumnName("tax_amount")
            .IsRequired();
        
        builder
            .Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired();
        
        builder
            .Property(x => x.CancellationReason)
            .HasColumnName("cancellation_reason")
            .IsRequired(false);
        
        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_date")
            .IsRequired();
        
        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("updated_date")
            .IsRequired();
        
        builder
            .Property(x => x.ShippedAt)
            .HasColumnName("shipped_date")
            .IsRequired();
        
        builder
            .Property(x => x.CancelledAt)
            .HasColumnName("cancelled_date")
            .IsRequired();

        builder
            .Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder
            .Property(x => x.PromoCodeId)
            .HasColumnName("promo_code_id")
            .IsRequired();

        builder
            .HasOne(x => x.PromoCode)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.PromoCodeId);
    }
}