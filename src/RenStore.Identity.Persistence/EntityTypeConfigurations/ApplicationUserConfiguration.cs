// Persistence/EntityTypeConfiguration/ApplicationUserConfiguration.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Identity.Domain.Enums;
using RenStore.Identity.Domain.ReadModels;

namespace RenStore.Identity.Persistence.EntityTypeConfigurations;

internal sealed class ApplicationUserConfiguration
    : IEntityTypeConfiguration<ApplicationUserReadModel>
{
    public void Configure(EntityTypeBuilder<ApplicationUserReadModel> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(x => x.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.FullName)
            .HasColumnName("full_name")
            .HasMaxLength(101)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.EmailConfirmed)
            .HasColumnName("email_confirmed")
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(x => x.Phone)
            .HasColumnName("phone")
            .HasMaxLength(20);

        builder.Property(x => x.PhoneConfirmed)
            .HasColumnName("phone_confirmed")
            .IsRequired();

        builder.Property(x => x.AccessFailedCount)
            .HasColumnName("access_failed_count")
            .IsRequired();

        builder.Property(x => x.LockoutEnd)
            .HasColumnName("lockout_end");

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasMaxLength(32)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<ApplicationUserStatus>(v))
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasIndex(x => x.Email)
            .IsUnique()
            .HasDatabaseName("ix_users_email");

        builder.HasIndex(x => x.Status)
            .HasDatabaseName("ix_users_status");

        builder.HasMany(x => x.Roles)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}