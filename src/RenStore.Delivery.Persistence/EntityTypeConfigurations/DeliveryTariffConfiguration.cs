using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Persistence.EntityTypeConfigurations;

public class DeliveryTariffConfiguration : IEntityTypeConfiguration<DeliveryTariff>
{
    public void Configure(EntityTypeBuilder<DeliveryTariff> builder)
    {
        builder
            .ToTable("delivery_tariffs");

        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("delivery_tariff_id")
            .HasColumnType("integer")
            .ValueGeneratedOnAdd()
            .IsRequired();
        
        // Value Object: Price
        builder
            .OwnsOne(x => x.Price, price =>
            {
                price
                    .Property(p => p.Amount)
                    .HasColumnName("price")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                
                price
                    .Property(p => p.Currency)
                    .HasColumnName("currency")
                    .HasConversion<string>()
                    .HasMaxLength(3)
                    .IsRequired();
            });
        
        // Value Object: WightLimitKg 
        builder
            .OwnsOne(x => x.WeightLimitKg, limit =>
            {
                limit
                    .Property(x => x.Kilograms)
                    .HasColumnName("weight_limit_kg")
                    .HasColumnType("decimal(10,2)")
                    .HasPrecision(10,2)
                    .IsRequired();
            });
        
        builder
            .Property(x => x.Type)
            .HasConversion<string>()
            .HasColumnName("type")
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasColumnName("description")
            .HasColumnType("varchar(500)")
            .HasMaxLength(500)
            .IsRequired(false);
        
        builder
            .Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .HasColumnType("BOOLEAN")
            .HasDefaultValue(false)
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
            .HasMany(x => x.DeliveryOrders)
            .WithOne("_tariff") 
            .HasForeignKey(x => x.DeliveryTariffId)
            .IsRequired(false);
        
        builder
            .HasIndex(x => x.IsDeleted)
            .HasMethod("btree")
            .HasDatabaseName("idx_delivery_tariffs_is_deleted_btree");
        
        /*builder
            .HasIndex(x => x.WeightLimitKg)
            .HasMethod("btree")
            .HasDatabaseName("idx_delivery_tariffs_weight_limit_kg_btree");*/
        
        builder
            .HasIndex(x => x.Type)
            .HasMethod("btree")
            .HasDatabaseName("idx_delivery_tariffs_type_btree");
    }
}