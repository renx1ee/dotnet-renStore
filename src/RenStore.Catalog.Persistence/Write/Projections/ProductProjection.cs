using RenStore.Catalog.Domain.Aggregates.Product;

namespace RenStore.Catalog.Persistence.Write.Projections;

public class ProductProjection
    : RenStore.Catalog.Application.Abstractions.Projections.IProductProjection
{
    private readonly CatalogDbContext _context;
    
    public ProductProjection(CatalogDbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task<Guid> AddAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(product);

        await _context.Products.AddAsync(product, cancellationToken);

        return product.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<Product> products,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(products);
        
        var productsList = products as IList<Product> ?? products.ToList();

        if (productsList.Count == 0) return;

        await _context.Products.AddRangeAsync(productsList, cancellationToken);
    }

    public void Remove(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        _context.Products.Remove(product);
    }
    
    public void RemoveRange(IReadOnlyCollection<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        _context.Products.RemoveRange(products);
    }
}