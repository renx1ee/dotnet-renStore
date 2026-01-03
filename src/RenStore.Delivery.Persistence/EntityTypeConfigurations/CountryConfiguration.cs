using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Persistence.EntityTypeConfigurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder
            .ToTable("countries");
        
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("country_id");
        
        builder
            .Property(x => x.Name)
            .HasColumnName("country_name")
            .HasMaxLength(256)
            .HasColumnType("varchar(256)")
            .IsRequired();
        
        builder
            .Property(x => x.NormalizedName)
            .HasColumnName("normalized_country_name")
            .HasMaxLength(256)
            .HasColumnType("varchar(256)")
            .IsRequired();
        
        builder
            .HasIndex(x => x.NormalizedName)
            .IsUnique();
        
        builder
            .Property(x => x.NameRu)
            .HasColumnName("country_name_ru")
            .HasMaxLength(256)
            .HasColumnType("varchar(256)")
            .IsRequired();
        
        builder
            .Property(x => x.NormalizedNameRu)
            .HasColumnName("normalized_country_name_ru")
            .HasMaxLength(256)
            .HasColumnType("varchar(256)")
            .IsRequired();
        
        builder
            .HasIndex(x => x.NormalizedNameRu)
            .IsUnique();
        
        builder
            .Property(x => x.Code)
            .HasColumnName("country_code")
            .HasMaxLength(5)
            .HasColumnType("varchar(5)")
            .IsRequired();
        
        builder
            .Property(x => x.PhoneCode)
            .HasColumnName("country_phone_code")
            .HasMaxLength(5)
            .HasColumnType("varchar(5)")
            .IsRequired(false);
        
        builder
            .HasIndex(x => x.Code)
            .IsUnique();

        /*builder
            .HasMany(x => x.ProductDetails)
            .WithOne(x => x.CountryOfManufacture)
            .HasForeignKey(x => x.CountryOfManufactureId);*/
    }
}