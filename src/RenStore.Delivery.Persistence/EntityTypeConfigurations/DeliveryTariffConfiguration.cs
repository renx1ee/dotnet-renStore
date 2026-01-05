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
            .IsRequired();

        builder
            .Property(x => x.Price)
            .HasColumnName("price")
            .IsRequired();

        builder
            .Property(x => x.Type)
            .HasColumnName("type")
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasColumnName("description")
            .IsRequired(false);

        builder
            .Property(x => x.WeightLimitKg)
            .HasColumnName("weight_limit_kg")
            .IsRequired();
        
        builder
            .Property(x => x.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false)
            .IsRequired();
        
        builder
            .Property(x => x.CreatedAt)
            .HasColumnName("created_date")
            .IsRequired();
        
        builder
            .Property(x => x.UpdatedAt)
            .HasColumnName("updated_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.DeletedAt)
            .HasColumnName("deleted_date")
            .IsRequired(false);
        
        builder
            .Property(x => x.DeliveryOrderId)
            .HasColumnName("delivery_order_id")
            .IsRequired();
    }
}