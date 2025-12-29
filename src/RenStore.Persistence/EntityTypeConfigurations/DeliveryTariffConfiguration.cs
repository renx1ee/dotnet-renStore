using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Domain.Entities;

namespace RenStore.Persistence.EntityTypeConfigurations;

public class DeliveryTariffConfiguration : IEntityTypeConfiguration<DeliveryTariffEntity>
{
    public void Configure(EntityTypeBuilder<DeliveryTariffEntity> builder)
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
    }
}