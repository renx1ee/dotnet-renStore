using RenStore.Catalog.Domain.Aggregates.Product;

namespace RenStore.Catalog.Domain.Interfaces;

public interface IProductRepository
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