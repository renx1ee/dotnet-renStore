using RenStore.Catalog.Domain.Aggregates.Product;

namespace RenStore.Catalog.Domain.Interfaces.Repository;

public interface IProductRepository
{
    Task<Product?> GetAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    Task SaveAsync(
        Product product,
        CancellationToken cancellationToken);
}