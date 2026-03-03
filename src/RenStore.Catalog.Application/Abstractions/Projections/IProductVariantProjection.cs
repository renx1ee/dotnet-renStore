using RenStore.Catalog.Domain.Aggregates.Variant;

namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IProductVariantProjection
{
    Task<Guid> AddAsync(
        ProductVariant variant,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<ProductVariant> variants,
        CancellationToken cancellationToken);

    void Remove(ProductVariant variant);

    void RemoveRange(IReadOnlyCollection<ProductVariant> variants);
}