using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Persistence.EntityTypeConfigurations;

internal sealed class CountryConfiguration
    : IEntityTypeConfiguration<CountryReadModel>
{
    public void Configure(EntityTypeBuilder<CountryReadModel> builder)
    {
        builder.ToTable("countries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("country_id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.NormalizedName)
            .HasColumnName("normalized_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.NameRu)
            .HasColumnName("name_ru")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.NormalizedNameRu)
            .HasColumnName("normalized_name_ru")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Code)
            .HasColumnName("code")
            .HasMaxLength(2)
            .IsRequired();

        builder.Property(x => x.PhoneCode)
            .HasColumnName("phone_code")
            .HasMaxLength(4)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasIndex(x => x.NormalizedName)
            .IsUnique()
            .HasDatabaseName("ix_countries_normalized_name");

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("ix_countries_code");
    }
}