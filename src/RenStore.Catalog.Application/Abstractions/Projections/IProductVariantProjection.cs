using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IProductVariantProjection
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        ProductVariantReadModel variant,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<ProductVariantReadModel> variants,
        CancellationToken cancellationToken);

    void Remove(ProductVariantReadModel variant);

    void RemoveRange(IReadOnlyCollection<ProductVariantReadModel> variants);
}