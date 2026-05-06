using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Identity.Domain.ReadModels;

namespace RenStore.Identity.Persistence.EntityTypeConfigurations;

internal sealed class PendingEmailVerificationConfiguration
    : IEntityTypeConfiguration<PendingEmailVerificationReadModel>
{
    public void Configure(
        EntityTypeBuilder<PendingEmailVerificationReadModel> builder)
    {
        builder.ToTable("pending_email_verifications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.Token)
            .HasColumnName("token")
            .IsRequired();

        builder.Property(x => x.IsUsed)
            .HasColumnName("is_used")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();

        builder.HasIndex(x => x.Token)
            .IsUnique()
            .HasDatabaseName("ix_pending_email_verifications_token");

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("ix_pending_email_verifications_user_id");
    }
}