namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IVariantImageProjection
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        VariantImageReadModel image,
        CancellationToken cancellationToken);
    
    Task AddRangeAsync(
        IReadOnlyCollection<VariantImageReadModel> images,
        CancellationToken cancellationToken);

    Task MarkAsMain(
        DateTimeOffset now,
        Guid imageId,
        CancellationToken cancellationToken);

    Task UnmarkAsMain(
        DateTimeOffset now,
        Guid variantId,
        CancellationToken cancellationToken);

    Task SoftDelete(
        DateTimeOffset now,
        Guid imageId,
        CancellationToken cancellationToken);
    
    void Remove(VariantImageReadModel image);
    
    void RemoveRange(IReadOnlyCollection<VariantImageReadModel> images);
}