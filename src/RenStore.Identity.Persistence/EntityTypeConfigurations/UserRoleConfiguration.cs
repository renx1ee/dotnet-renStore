using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Identity.Domain.ReadModels;

namespace RenStore.Identity.Persistence.EntityTypeConfigurations;

internal sealed class UserRoleConfiguration
    : IEntityTypeConfiguration<UserRoleReadModel>
{
    public void Configure(EntityTypeBuilder<UserRoleReadModel> builder)
    {
        builder.ToTable("user_roles");

        builder.HasKey(x => new { x.UserId, x.RoleId });

        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(x => x.RoleId)
            .HasColumnName("role_id")
            .IsRequired();

        builder.Property(x => x.RoleName)
            .HasColumnName("role_name")
            .HasMaxLength(64)
            .IsRequired();

        builder.HasIndex(x => x.UserId)
            .HasDatabaseName("ix_user_roles_user_id");
    }
}