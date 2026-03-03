using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Variant;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

public class ProductVariantRepository
    : RenStore.Catalog.Domain.Interfaces.Repository.IProductVariantRepository
{
    private readonly IEventStore _eventStore;
    
    public ProductVariantRepository(IEventStore eventStore) =>
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    
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

    public async Task SaveAsync(
        ProductVariant productVariant,
        CancellationToken cancellationToken)
    {
        // TODO:
    }
}