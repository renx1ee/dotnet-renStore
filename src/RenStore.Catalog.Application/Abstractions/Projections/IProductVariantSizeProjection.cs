namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IProductVariantSizeProjection
{
    Task CommitAsync(CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        VariantSizeReadModel variant,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<VariantSizeReadModel> variants,
        CancellationToken cancellationToken);

    Task SoftDeleteAsync(
        Guid variantId,
        Guid sizeId,
        DateTimeOffset removedAt,
        CancellationToken cancellationToken);

    Task RestoreAsync(
        Guid variantId,
        Guid sizeId,
        DateTimeOffset restoredAt,
        CancellationToken cancellationToken);

    void Remove(VariantSizeReadModel variant);

    void RemoveRange(IReadOnlyCollection<VariantSizeReadModel> variants);
}