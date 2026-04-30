using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Persistence.EntityTypeConfigurations;

internal sealed class PaymentAttemptConfiguration : IEntityTypeConfiguration<PaymentAttemptReadModel>
{
    public void Configure(EntityTypeBuilder<PaymentAttemptReadModel> builder)
    {
        builder
            .ToTable("payment_attempts");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("attempt_id")
            .IsRequired();

        builder
            .Property(x => x.PaymentId)
            .HasColumnName("payment_id")
            .IsRequired();

        builder
            .Property(x => x.AttemptNumber)
            .HasColumnName("attempt_number")
            .IsRequired();

        builder
            .Property(x => x.IsSuccessful)
            .HasColumnName("is_successful")
            .IsRequired();

        builder
            .Property(x => x.FailureReason)
            .HasColumnName("failure_reason")
            .HasMaxLength(500);

        builder
            .Property(x => x.ErrorCode)
            .HasColumnName("error_code")
            .HasMaxLength(64);

        builder
            .Property(x => x.ExternalAuthCode)
            .HasColumnName("external_auth_code")
            .HasMaxLength(256);

        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder
            .Property(x => x.ResolvedAt)
            .HasColumnName("resolved_at");

        builder
            .HasIndex(x => x.PaymentId)
            .HasDatabaseName("ix_payment_attempts_payment_id");
    }
}