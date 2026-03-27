using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RenStore.Inventory.Domain.ReadModels;

namespace RenStore.Inventory.Persistence.EntityTypeConfigurations;

internal sealed class VariantStockReadModelConfiguration
    : IEntityTypeConfiguration<VariantStockReadModel>
{
    public void Configure(EntityTypeBuilder<VariantStockReadModel> builder)
    {
    }
}