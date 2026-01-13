using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.ValueObjects;

namespace RenStore.Delivery.Persistence.EntityTypeConfigurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder
            .ToTable("addresses");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("address_id")
            .HasColumnType("uuid")
            .HasDefaultValueSql("gen_random_uuid()")
            .IsRequired();
        
        builder
            .Property(x => x.HouseCode)
            .HasColumnName("house_code")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired(false);
        
        builder
            .Property(x => x.Street)
            .HasColumnName("street")
            .HasColumnType("varchar(150)")
            .HasMaxLength(150)
            .IsRequired();
        
        builder
            .Property(x => x.BuildingNumber)
            .HasColumnName("building_number")
            .HasColumnType("varchar(5)")
            .HasMaxLength(5)
            .IsRequired(false); // TODO: проверить, обязателен ли
        
        builder
            .Property(x => x.ApartmentNumber)
            .HasColumnName("apartment_number")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired(false);
        
        builder
            .Property(x => x.Entrance)
            .HasColumnName("entrance")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired(false);
        
        builder
            .Property(x => x.Floor)
            .HasColumnName("floor")
            .HasColumnType("int")
            .IsRequired(false);

        builder
            .OwnsOne<FullMultiplyAddress>("_fullAddress", address =>
            {
                address
                    .Property("English")
                    .HasColumnName("full_address_en")
                    .HasColumnType("varchar(500)")
                    .HasMaxLength(500)
                    .IsRequired();
                
                address
                    .Property("Russian")
                    .HasColumnName("full_address_ru")
                    .HasColumnType("varchar(500)")
                    .HasMaxLength(500)
                    .IsRequired();
            })
            .Navigation("_fullAddress")
            .IsRequired();
        
        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_date")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'")
            .IsRequired();
        
        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("updated_date")
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);
        
        builder
            .Property(x => x.DeletedAt)
            .HasColumnName("deleted_date")
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);

        builder
            .Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .HasColumnType("boolean")
            .HasDefaultValueSql("false")
            .IsRequired();

        /*builder
            .HasOne(x => x.ApplicationUser)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.ApplicationUserId);*/
        
        builder
            .Property(x => x.ApplicationUserId)
            .HasColumnName("user_id")
            .HasColumnType("uuid")
            .IsRequired();

        builder
            .HasOne(typeof(Country), "_country")
            .WithMany()
            .HasForeignKey("CountryId")
            .IsRequired();
        
        builder
            .Property(x => x.CountryId)
            .HasColumnName("country_id")
            .HasColumnType("int")
            .IsRequired();
        
        builder
            .HasOne<City>("_city")
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.CityId)
            .IsRequired(false);
        
        builder
            .Property(x => x.CityId)
            .HasColumnName("city_id")
            .HasColumnType("int")
            .IsRequired();
        
        builder
            .HasQueryFilter(x => x.IsDeleted);

        builder
            .HasIndex(x => x.ApplicationUserId)
            .HasMethod("btree")
            .HasDatabaseName("idx_address_application_user_id_btree");
        
        builder
            .HasIndex(x => x.CityId)
            .HasMethod("btree")
            .HasDatabaseName("idx_address_city_id_btree");
        
        builder
            .HasIndex(x => x.CountryId)
            .HasMethod("btree")
            .HasDatabaseName("idx_address_country_id_btree");
        
        builder
            .HasIndex(x => x.IsDeleted)
            .HasMethod("btree")
            .HasDatabaseName("idx_address_is_deleted_btree");
    }
}