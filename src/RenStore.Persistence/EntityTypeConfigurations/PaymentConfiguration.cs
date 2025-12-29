using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class PaymentConfiguration : IEntityTypeConfiguration<PaymentEntity>
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        builder
            .ToTable("payments");

        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .HasColumnName("payment_id");

        builder
            .Property(x => x.Amount)
            .HasColumnName("amount")
            .IsRequired();

        builder
            .Property(x => x.OriginalAmount)
            .HasColumnName("original_amount")
            .IsRequired();
        
        builder
            .Property(x => x.Commission)
            .HasColumnName("commission")
            .IsRequired();
        
        builder
            .Property(x => x.TaxAmount)
            .HasColumnName("tax_amount")
            .IsRequired();
        
        builder
            .Property(x => x.RefundedAmount)
            .HasColumnName("refunded_amount")
            .IsRequired();

        builder
            .Property(x => x.Currency)
            .HasColumnName("currency")
            .IsRequired();
        
        builder
            .Property(x => x.IsSuccess)
            .HasColumnName("is_success")
            .IsRequired(false);

        builder
            .Property(x => x.Method)
            .HasColumnName("method")
            .IsRequired();
        
        builder
            .Property(x => x.MethodDetails)
            .HasColumnName("method_details")
            .IsRequired(false);
        
        builder
            .Property(x => x.Status)
            .HasColumnName("status")
            .IsRequired();
        
        builder
            .Property(x => x.ErrorCode)
            .HasColumnName("error_code")
            .IsRequired(false);
        
        builder
            .Property(x => x.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValue(DateTime.UtcNow)
            .IsRequired();
        
        builder
            .Property(x => x.UpdatedDate)
            .HasColumnName("updated_date")
            .IsRequired();
          
        builder
            .Property(x => x.PaymentDate)
            .HasColumnName("payment_date")
            .IsRequired();
        
        builder
            .Property(x => x.AuthorizedDate)
            .HasColumnName("authorized_date")
            .IsRequired();
        
        builder
            .Property(x => x.CapturedDate)
            .HasColumnName("captured_date")
            .IsRequired();
        
        builder
            .Property(x => x.RefundedDate)
            .HasColumnName("refunded_date")
            .IsRequired();
        
        builder
            .Property(x => x.FailedDate)
            .HasColumnName("failed_date")
            .IsRequired();
        
        builder
            .Property(x => x.ExpiryDate)
            .HasColumnName("expiry_date")
            .IsRequired();
        
        builder
            .Property(x => x.OrderId)
            .HasColumnName("order_id")
            .IsRequired();
        
        builder
            .HasOne(x => x.Order)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.OrderId);
        
    }
}