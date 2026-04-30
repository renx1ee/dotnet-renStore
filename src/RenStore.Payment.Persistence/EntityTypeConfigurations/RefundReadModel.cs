using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Payment.Domain.Enums;
using RenStore.Payment.Domain.ReadModels;
using RenStore.Payment.Persistence.EntityTypeConfigurations.Conversions;

namespace RenStore.Payment.Persistence.EntityTypeConfigurations;

internal sealed class RefundConfiguration : IEntityTypeConfiguration<RefundReadModel>
{
    public void Configure(EntityTypeBuilder<RefundReadModel> builder)
    {
        builder
            .ToTable("refunds");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("refund_id")
            .IsRequired();

        builder
            .Property(x => x.PaymentId)
            .HasColumnName("payment_id")
            .IsRequired();

        builder
            .Property(x => x.Amount)
            .HasColumnName("amount")
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
            .Property(x => x.Reason)
            .HasColumnName("reason")
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(32)
            .HasConversion(
                v => RefundStatusConversion.RefundStatusToDatabase(v),
                v => RefundStatusConversion.RefundStatusFromDatabase(v))
            .IsRequired();

        builder
            .Property(x => x.ExternalRefundId)
            .HasColumnName("external_refund_id")
            .HasMaxLength(256);

        builder
            .Property(x => x.FailureReason)
            .HasColumnName("failure_reason")
            .HasMaxLength(500);

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder
            .Property(x => x.ResolvedAt)
            .HasColumnName("resolved_at");

        builder
            .HasIndex(x => x.PaymentId)
            .HasDatabaseName("ix_refunds_payment_id");

        builder
            .HasIndex(x => x.Status)
            .HasDatabaseName("ix_refunds_status");
    }
}