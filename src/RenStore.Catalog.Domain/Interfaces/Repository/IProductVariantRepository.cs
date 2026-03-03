using RenStore.Catalog.Domain.Aggregates.Variant;

namespace RenStore.Catalog.Domain.Interfaces.Repository;

public interface IProductVariantRepository
{
    Task<ProductVariant?> GetAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    Task SaveAsync(
        ProductVariant productVariant,
        CancellationToken cancellationToken);
}