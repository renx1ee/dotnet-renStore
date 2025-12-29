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
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("address_id");

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
            .Property(x => x.CreatedDate)
            .HasColumnName("created_date")
            .HasDefaultValue(DateTime.UtcNow)
            .IsRequired();

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