using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class AddressConfiguration : IEntityTypeConfiguration<AddressEntity>
{
    public void Configure(EntityTypeBuilder<AddressEntity> builder)
    {
        builder
            .ToTable("addresses");

        builder
            .HasQueryFilter(x => !x.IsDeleted);

        builder
            .HasIndex(x => x.ApplicationUserId);
        
        builder
            .HasIndex(x => x.CityId);
        
        builder
            .HasIndex(x => x.CountryId);
        
        builder
            .HasIndex(x => x.IsDeleted);

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("address_id")
            .HasColumnType("uuid");

        builder
            .Property(x => x.HouseCode)
            .HasColumnName("house_code")
            .HasMaxLength(50)
            .IsRequired(false);
        
        builder
            .Property(x => x.Street)
            .HasColumnName("street")
            .HasMaxLength(150)
            .IsRequired();
        
        builder
            .Property(x => x.BuildingNumber)
            .HasColumnName("building_number")
            .HasMaxLength(50)
            .IsRequired(false);
        
        builder
            .Property(x => x.ApartmentNumber)
            .HasColumnName("apartment_number")
            .HasMaxLength(50)
            .IsRequired(false);
        
        builder
            .Property(x => x.Entrance)
            .HasColumnName("entrance")
            .HasMaxLength(50)
            .IsRequired(false);
        
        builder
            .Property(x => x.Floor)
            .HasColumnName("floor")
            .IsRequired(false);
        
        builder
            .Property(x => x.FlatNumber)
            .HasColumnName("flat_number")
            .HasMaxLength(50)
            .IsRequired(false);
        
        builder
            .Property(x => x.FullAddress)
            .HasColumnName("full_address")
            .HasMaxLength(500)
            .IsRequired();
        
        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_date")
            .HasDefaultValueSql("now()")
            .IsRequired();
        
        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("updated_date")
            .IsRequired(false);

        builder
            .Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .HasColumnName("is_deleted")
            .IsRequired(false);

        builder
            .HasOne(x => x.ApplicationUser)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.ApplicationUserId);
        
        builder
            .Property(x => x.ApplicationUserId)
            .HasColumnName("user_id")
            .IsRequired();
        
        builder
            .HasOne(x => x.Country)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.CountryId);
        
        builder
            .Property(x => x.CountryId)
            .HasColumnName("country_id")
            .IsRequired();
        
        builder
            .HasOne(x => x.City)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.CityId);
        
        builder
            .Property(x => x.CityId)
            .HasColumnName("city_id")
            .IsRequired();
    }
}