using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IProductProjection
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        ProductReadModel product,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<ProductReadModel> products,
        CancellationToken cancellationToken);

    Task SoftDeleteAsync(
        Guid productId,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    Task ApproveAsync(
        Guid productId,
        DateTimeOffset now,
        CancellationToken cancellationToken);
    
    Task RejectAsync(
        Guid productId,
        DateTimeOffset now,
        CancellationToken cancellationToken);
    
    Task HideAsync(
        Guid productId,
        DateTimeOffset now,
        CancellationToken cancellationToken);
    
    Task ArchiveAsync(
        Guid productId,
        DateTimeOffset now,
        CancellationToken cancellationToken);
    
    Task DraftAsync(
        Guid productId,
        DateTimeOffset now,
        CancellationToken cancellationToken);

    void Remove(ProductReadModel product);

    void RemoveRange(IReadOnlyCollection<ProductReadModel> products);
}