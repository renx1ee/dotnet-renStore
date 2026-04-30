using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Payment.Domain.Enums;
using RenStore.Payment.Domain.ReadModels;
using RenStore.Payment.Persistence.EntityTypeConfigurations.Conversions;

namespace RenStore.Payment.Persistence.EntityTypeConfigurations;

internal sealed class PaymentConfiguration : IEntityTypeConfiguration<PaymentReadModel>
{
    public void Configure(EntityTypeBuilder<PaymentReadModel> builder)
    {
        builder
            .ToTable("payments");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("payment_id")
            .IsRequired();

        builder
            .Property(x => x.OrderId)
            .HasColumnName("order_id")
            .IsRequired();

        builder
            .Property(x => x.CustomerId)
            .HasColumnName("customer_id")
            .IsRequired();

        builder
            .Property(x => x.Amount)
            .HasColumnName("amount")
            .HasPrecision(18, 2)
            .IsRequired();

        builder
            .Property(x => x.RefundedAmount)
            .HasColumnName("refunded_amount")
            .HasPrecision(18, 2)
            .IsRequired();

        builder
            .Property(x => x.Currency)
            .HasColumnName("currency")
            .HasMaxLength(8)
            .HasConversion(
                v => CurrencyConversion.CurrencyToDatabase(v),
                v => CurrencyConversion.CurrencyFromDatabase(v))
            .IsRequired();

        builder 
            .Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(32)
            .HasConversion(
                v => StatusConversion.PaymentStatusToDatabase(v),
                v => StatusConversion.PaymentStatusFromDatabase(v))
            .IsRequired();

        builder
            .Property(x => x.Provider)
            .HasColumnName("provider")
            .HasMaxLength(32)
            .HasConversion(
                v => PaymentProviderConversion.PaymentProviderToDatabase(v),
                v => PaymentProviderConversion.PaymentProviderFromDatabase(v))
            .IsRequired();

        builder
            .Property(x => x.PaymentMethod)
            .HasColumnName("payment_method")
            .HasMaxLength(32)
            .HasConversion(
                v => PaymentMethodConversion.PaymentMethodToDatabase(v),
                v => PaymentMethodConversion.PaymentMethodFromDatabase(v))
            .IsRequired();

        builder
            .Property(x => x.ExternalPaymentId)
            .HasColumnName("external_payment_id")
            .HasMaxLength(256);

        builder
            .Property(x => x.FailureReason)
            .HasColumnName("failure_reason")
            .HasMaxLength(500);

        builder
            .Property(x => x.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();

        builder
            .Property(x => x.CapturedAt)
            .HasColumnName("captured_at");

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        builder
            .HasIndex(x => x.OrderId)
            .HasDatabaseName("ix_payments_order_id");

        builder
            .HasIndex(x => x.CustomerId)
            .HasDatabaseName("ix_payments_customer_id");

        builder
            .HasIndex(x => x.Status)
            .HasDatabaseName("ix_payments_status");

        builder
            .HasIndex(x => x.ExternalPaymentId)
            .HasDatabaseName("ix_payments_external_payment_id");
    }
}