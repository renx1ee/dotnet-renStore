using RenStore.Inventory.Domain.ReadModels;

namespace RenStore.Inventory.Application.Abstractions.ReadRepository;

public interface IStockReadRepository
{
    Task<VariantStockReadModel?> GetAsync(
        Guid stockId,
        CancellationToken cancellationToken);
    
    Task<VariantStockReadModel?> GetAsync(
        Guid variantId,
        Guid sizeId,
        CancellationToken cancellationToken);
}