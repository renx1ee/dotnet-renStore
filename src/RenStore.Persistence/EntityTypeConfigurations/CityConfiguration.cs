using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class CityConfiguration : IEntityTypeConfiguration<CityEntity>
{
    public void Configure(EntityTypeBuilder<CityEntity> builder)
    {
        builder
            .ToTable("cities");
        
        builder
            .HasKey(x => x.Id);
        
        builder
            .Property(x => x.Id)
            .HasColumnName("city_id");
        
        builder
            .Property(x => x.Name)
            .HasColumnName("city_name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .Property(x => x.NormalizedName)
            .HasColumnName("normalized_city_name")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .Property(x => x.NameRu)
            .HasColumnName("city_name_ru")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .Property(x => x.NormalizedNameRu)
            .HasColumnName("normalized_city_name_ru")
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(x => x.CountryId)
            .HasColumnName("country_id")
            .IsRequired();
        
        builder
            .HasOne(x => x.Country)
            .WithMany(x => x.Cities)
            .HasForeignKey(x => x.CountryId);
    }
}