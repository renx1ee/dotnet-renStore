using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Application.Abstractions.Projections;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Interfaces.Repository;

namespace RenStore.Catalog.Persistence.Write.Projections;

public class ProductVariantProjection
    : RenStore.Catalog.Application.Abstractions.Projections.IProductVariantProjection
{
    private readonly CatalogDbContext _context;
    
    public ProductVariantProjection(CatalogDbContext context) =>
        _context = context       ?? throw new ArgumentNullException(nameof(context));
    
    public async Task<Guid> AddAsync(
        ProductVariant variant,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(variant);

        await _context.Variants.AddAsync(variant, cancellationToken);

        return variant.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<ProductVariant> variants,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(variants);

        var variantsList = variants as IList<ProductVariant> ?? variants.ToList();

        if (variantsList.Count == 0) return;
        
        await _context.Variants.AddRangeAsync(variantsList, cancellationToken);
    }

    public void Remove(ProductVariant variant)
    {
        ArgumentNullException.ThrowIfNull(variant);

        _context.Variants.Remove(variant);
    }
    
    public void RemoveRange(IReadOnlyCollection<ProductVariant> variants)
    {
        ArgumentNullException.ThrowIfNull(variants);

        _context.Variants.RemoveRange(variants);
    }
}