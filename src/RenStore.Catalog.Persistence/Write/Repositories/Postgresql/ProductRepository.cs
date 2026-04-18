using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Product;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

internal sealed class ProductRepository
    : RenStore.Catalog.Domain.Interfaces.Repository.IProductRepository
{
    private readonly IEventStore _eventStore;
    
    public ProductRepository(
        IEventStore eventStore)
    { 
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

    public async Task SaveAsync(
        Product product,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(product);

        var uncommittedEvents = product.GetUncommittedEvents();

        if (uncommittedEvents.Count == 0)
            return;
        
        await _eventStore.AppendAsync(
            aggregateId: product.Id,
            expectedVersion: product.Version,
            events: uncommittedEvents.ToList(),
            cancellationToken: cancellationToken);
        
        product.UncommittedEventsClear();
    }
}