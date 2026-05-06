using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Persistence.EntityTypeConfigurations;

internal sealed class CityConfiguration
    : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("cities");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("city_id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.NameRu)
            .HasColumnName("name_ru")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.NormalizedName)
            .HasColumnName("normalized_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.NormalizedNameRu)
            .HasColumnName("normalized_name_ru")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .IsRequired();

        builder.Property(x => x.CountryId)
            .HasColumnName("country_id")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(x => x.DeletedAt)
            .HasColumnName("deleted_at");

        builder.HasIndex(x => x.CountryId)
            .HasDatabaseName("ix_cities_country_id");

        builder.HasIndex(x => new { x.NormalizedName, x.CountryId })
            .IsUnique()
            .HasDatabaseName("ix_cities_normalized_name_country_id");
    }
}