using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Persistence.EntityTypeConfigurations;

internal sealed class DeliveryTariffConfiguration
    : IEntityTypeConfiguration<DeliveryTariffReadModel>
{
    public void Configure(EntityTypeBuilder<DeliveryTariffReadModel> builder)
    {
        builder.ToTable("delivery_tariffs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("tariff_id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.PriceAmount)
            .HasColumnName("price_amount")
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.Currency)
            .HasColumnName("currency")
            .HasMaxLength(8)
            .IsRequired();

        builder.Property(x => x.WeightLimitKg)
            .HasColumnName("weight_limit_kg")
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasColumnName("type")
            .HasMaxLength(64)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<DeliveryTariffType>(v))
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

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
    }
}