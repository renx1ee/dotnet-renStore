using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Persistence.Write.Projections;

internal sealed class ProductProjection
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
    
    public async Task PublishAsync(
        Guid productId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var view = await _context.Products
            .FindAsync(productId, cancellationToken);

        if (view is null) return;

        view.UpdatedAt = now;
        view.Status = ProductStatus.Published;
    }

    public async Task SoftDeleteAsync(
        Guid productId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var view = await _context.Products
            .FindAsync(productId, cancellationToken);

        if (view is null) return;

        view.DeletedAt = now;
        view.Status = ProductStatus.Deleted;
    }
    
    public async Task ApproveAsync(
        Guid productId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var view = await _context.Products
            .FindAsync(productId, cancellationToken);

        if (view is null) return;

        view.UpdatedAt = now;
        view.Status = ProductStatus.Approved;
    }
    
    public async Task RejectAsync(
        Guid productId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var view = await _context.Products
            .FindAsync(productId, cancellationToken);

        if (view is null) return;

        view.UpdatedAt = now;
        view.Status = ProductStatus.Rejected;
    }
    
    public async Task ArchiveAsync(
        Guid productId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var view = await _context.Products
            .FindAsync(productId, cancellationToken);

        if (view is null) return;

        view.UpdatedAt = now;
        view.Status = ProductStatus.Archived;
    }
    
    public async Task HideAsync(
        Guid productId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var view = await _context.Products
            .FindAsync(productId, cancellationToken);

        if (view is null) return;

        view.UpdatedAt = now;
        view.Status = ProductStatus.Hidden;
    }
    
    public async Task DraftAsync(
        Guid productId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var view = await _context.Products
            .FindAsync(productId, cancellationToken);

        if (view is null) return;

        view.UpdatedAt = now;
        view.Status = ProductStatus.Draft;
    }
    
    public async Task RestoreAsync(
        Guid productId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        var view = await _context.Products
            .FindAsync(productId, cancellationToken);

        if (view is null) return;

        view.UpdatedAt = now;
        view.DeletedAt = null;
        view.Status = ProductStatus.Archived;
    }

    public async Task<bool> ExistsAsync(
        Guid productId,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .AnyAsync(x => 
                x.Id == productId, 
                cancellationToken);
    }
    
    public async Task<bool> BelongAsync(
        Guid productId,
        long sellerId,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .AnyAsync(p => 
                p.Id == productId && 
                p.SellerId == sellerId,
                cancellationToken);
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