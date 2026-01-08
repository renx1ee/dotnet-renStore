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
            .HasColumnName("country_id")
            .HasColumnType("int")
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        builder
            .Property(x => x.Name)
            .HasColumnName("country_name")
            .HasColumnType("varchar(256)")
            .HasMaxLength(256)
            .IsRequired();
        
        builder
            .Property(x => x.NormalizedName)
            .HasColumnName("normalized_country_name")
            .HasColumnType("varchar(256)")
            .HasMaxLength(256)
            .IsRequired();
        
        builder
            .Property(x => x.NameRu)
            .HasColumnName("country_name_ru")
            .HasColumnType("varchar(256)")
            .HasMaxLength(256)
            .IsRequired();
        
        builder
            .Property(x => x.NormalizedNameRu)
            .HasColumnName("normalized_country_name_ru")
            .HasColumnType("varchar(256)")
            .HasMaxLength(256)
            .IsRequired();
        
        builder
            .Property(x => x.Code)
            .HasColumnName("country_code")
            .HasColumnType("varchar(2)")
            .HasMaxLength(2)
            .IsRequired();
        
        builder
            .Property(x => x.PhoneCode) 
            .HasColumnName("country_phone_code")
            .HasColumnType("varchar(3)")
            .HasMaxLength(3)
            .IsRequired(false);
        
        builder
            .Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .HasColumnType("boolean")
            .HasDefaultValueSql("false")
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
            .HasMany(x => x.Addresses)
            .WithOne()
            .HasForeignKey(x => x.CountryId);
        
        builder
            .HasMany(x => x.Cities)
            .WithOne("_country")
            .HasForeignKey(x => x.CountryId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        
        builder
            .HasIndex(x => x.NormalizedName)
            .HasMethod("btree")
            .HasDatabaseName("idx_country_normalized_name_btree")
            .IsUnique();
        
        builder
            .HasIndex(x => x.NormalizedNameRu)
            .HasMethod("btree")
            .HasDatabaseName("idx_country_normalized_name_ru_btree")
            .IsUnique();
        
        builder
            .HasIndex(x => x.Code)
            .HasMethod("btree")
            .HasDatabaseName("idx_country_code_btree")
            .IsUnique();
    }
    
}