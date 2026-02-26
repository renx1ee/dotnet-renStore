using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Domain.Interfaces.Repository;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

public class ProductRepository 
    : IProductRepository
{
    private readonly CatalogDbContext _context;
    private readonly IEventStore _eventStore;
    
    public ProductRepository(
        CatalogDbContext context,
        IEventStore eventStore)
    {
        _context = context       ?? throw new ArgumentNullException(nameof(context));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }
    
    public async Task<Product?> GetAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        
        var events = await _eventStore.LoadAsync(id, cancellationToken);
        
        if (!events.Any()) return null;
        
        return Product.Rehydrate(events);
    }

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