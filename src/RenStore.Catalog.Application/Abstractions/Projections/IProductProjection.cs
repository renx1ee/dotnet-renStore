using RenStore.Catalog.Domain.Aggregates.Product;

namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IProductProjection
{
    Task<Guid> AddAsync(
        Product product,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<Product> products,
        CancellationToken cancellationToken);

    void Remove(Product product);

    void RemoveRange(IReadOnlyCollection<Product> products);
}