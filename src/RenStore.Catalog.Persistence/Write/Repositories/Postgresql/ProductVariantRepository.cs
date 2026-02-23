using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Interfaces.Repository;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

public class ProductVariantRepository
    : IProductVariantRepository
{
    private readonly CatalogDbContext _context;
    private readonly IEventStore _eventStore;
    
    public ProductVariantRepository(
        CatalogDbContext context,
        IEventStore eventStore)
    {
        _context = context       ?? throw new ArgumentNullException(nameof(context));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }
    
    public async Task<ProductVariant?> GetAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        
        var events = await _eventStore.LoadAsync(id, cancellationToken);
        
        if (!events.Any()) return null;
        
        return ProductVariant.Rehydrate(events);
    }

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