using RenStore.Catalog.Domain.Aggregates.Variant;

namespace RenStore.Catalog.Domain.Interfaces.Repository;

public interface IProductVariantRepository
{
    Task<ProductVariant?> GetAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        ProductVariant variant,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<ProductVariant> variants,
        CancellationToken cancellationToken);

    void Remove(ProductVariant variant);

    void RemoveRange(IReadOnlyCollection<ProductVariant> variants);
}