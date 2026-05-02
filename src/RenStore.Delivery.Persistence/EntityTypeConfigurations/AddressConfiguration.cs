using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Persistence.EntityTypeConfigurations;

internal sealed class AddressConfiguration
    : IEntityTypeConfiguration<AddressReadModel>
{
    public void Configure(EntityTypeBuilder<AddressReadModel> builder)
    {
        builder.ToTable("addresses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("address_id")
            .IsRequired();

        builder.Property(x => x.ApplicationUserId)
            .HasColumnName("application_user_id")
            .IsRequired();

        builder.Property(x => x.CountryId)
            .HasColumnName("country_id")
            .IsRequired();

        builder.Property(x => x.CityId)
            .HasColumnName("city_id")
            .IsRequired();

        builder.Property(x => x.Street)
            .HasColumnName("street")
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(x => x.HouseCode)
            .HasColumnName("house_code")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.BuildingNumber)
            .HasColumnName("building_number")
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(x => x.ApartmentNumber)
            .HasColumnName("apartment_number")
            .HasMaxLength(32);

        builder.Property(x => x.Entrance)
            .HasColumnName("entrance")
            .HasMaxLength(32);

        builder.Property(x => x.Floor)
            .HasColumnName("floor");

        builder.Property(x => x.FullAddressEn)
            .HasColumnName("full_address_en")
            .HasMaxLength(1000);

        builder.Property(x => x.FullAddressRu)
            .HasColumnName("full_address_ru")
            .HasMaxLength(1000);

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

        builder.HasIndex(x => x.ApplicationUserId)
            .HasDatabaseName("ix_addresses_user_id");

        builder.HasIndex(x => x.CityId)
            .HasDatabaseName("ix_addresses_city_id");
    }
}