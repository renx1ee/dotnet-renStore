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

    void Remove(ProductReadModel product);

    void RemoveRange(IReadOnlyCollection<ProductReadModel> products);
}