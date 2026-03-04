using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Persistence.Write.Projections;

public class ProductProjection
    : RenStore.Catalog.Application.Abstractions.Projections.IProductProjection
{
    private readonly CatalogDbContext _context;
    
    public ProductProjection(CatalogDbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Guid> AddAsync(
        ProductReadModel product,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(product);
        
        await _context.Products.AddAsync(product, cancellationToken);
        
        return product.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<ProductReadModel> products,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(products);
        
        var productsList = products as IList<ProductReadModel> ?? products.ToList();

        if (productsList.Count == 0) return;

        await _context.Products.AddRangeAsync(productsList, cancellationToken);
    }

    public void Remove(ProductReadModel product)
    {
        ArgumentNullException.ThrowIfNull(product);

        _context.Products.Remove(product);
    }
    
    public void RemoveRange(IReadOnlyCollection<ProductReadModel> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        _context.Products.RemoveRange(products);
    }
}