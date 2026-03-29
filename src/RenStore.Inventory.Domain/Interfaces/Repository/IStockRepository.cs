using RenStore.Inventory.Domain.Aggregates.Stock;

namespace RenStore.Inventory.Domain.Interfaces.Repository;

public interface IStockRepository
{
    Task<VariantStock?> GetAsync(
        Guid stockId,
        CancellationToken cancellationToken);

    Task SaveAsync(
        VariantStock stock,
        CancellationToken cancellationToken);
}