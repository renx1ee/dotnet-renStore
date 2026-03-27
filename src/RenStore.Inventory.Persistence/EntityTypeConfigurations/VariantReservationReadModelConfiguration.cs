using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Inventory.Domain.ReadModels;

namespace RenStore.Inventory.Persistence.EntityTypeConfigurations;

internal sealed class VariantReservationReadModelConfiguration
    : IEntityTypeConfiguration<VariantReservationReadModel>
{
    public void Configure(EntityTypeBuilder<VariantReservationReadModel> builder)
    {
    }
}