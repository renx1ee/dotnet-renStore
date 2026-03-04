using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Persistence.Write.Projections;

internal sealed class ProductVariantProjection
    : RenStore.Catalog.Application.Abstractions.Projections.IProductVariantProjection
{
    private readonly CatalogDbContext _context;
    
    public ProductVariantProjection(CatalogDbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Guid> AddAsync(
        ProductVariantReadModel variant,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(variant);

        await _context.Variants.AddAsync(variant, cancellationToken);

        return variant.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<ProductVariantReadModel> variants,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(variants);

        var variantsList = variants as IList<ProductVariantReadModel> ?? variants.ToList();

        if (variantsList.Count == 0) return;
        
        await _context.Variants.AddRangeAsync(variantsList, cancellationToken);
    }

    public void Remove(ProductVariantReadModel variant)
    {
        ArgumentNullException.ThrowIfNull(variant);

        _context.Variants.Remove(variant);
    }
    
    public void RemoveRange(IReadOnlyCollection<ProductVariantReadModel> variants)
    {
        ArgumentNullException.ThrowIfNull(variants);

        _context.Variants.RemoveRange(variants);
    }
}