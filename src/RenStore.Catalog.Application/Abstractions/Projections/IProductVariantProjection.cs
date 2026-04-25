namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IProductVariantProjection
{
    Task CommitAsync(CancellationToken cancellationToken);
    
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

    Task ChangeDiscountAsync(
        Guid variantId,
        int discountPercents,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task ChangeReviewsCountAsync(
        Guid variantId,
        int reviewsCount,
        double averageRating,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task ChangeSellerVerificationAsync(
        Guid variantId,
        bool isVerified,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task ChangeStockAsync(
        Guid variantId,
        int stock,
        int sales,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task RestoreAsync(
        Guid variantId,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    void Remove(ProductVariantReadModel variant);

    void RemoveRange(IReadOnlyCollection<ProductVariantReadModel> variants);
}