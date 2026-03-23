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

    Task PublishAsync(
        Guid variantId,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task ArchiveAsync(
        Guid variantId,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task ChangeNameAsync(
        Guid variantId,
        string name,
        string normalizedName,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task DraftAsync(
        Guid variantId,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task SetMainImageIdAsyncAsync(
        Guid variantId,
        Guid imageId,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task SoftDeleteAsync(
        Guid variantId,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task RestoreAsync(
        Guid variantId,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    void Remove(ProductVariantReadModel variant);

    void RemoveRange(IReadOnlyCollection<ProductVariantReadModel> variants);
}