using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Media;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

internal sealed class VariantImageRepository
    : RenStore.Catalog.Domain.Interfaces.Repository.IVariantImageRepository   
{
    private readonly IEventStore _eventStore;
    
    public VariantImageRepository(
        IEventStore eventStore)
    { 
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }
    
    public async Task<VariantImage?> GetAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        
        var events = await _eventStore.LoadAsync(id, cancellationToken);

        if (!events.Any()) return null;
        
        return VariantImage.Rehydrate(events);
    }
    
    public async Task SaveAsync(
        VariantImage image,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(image);
        
        var uncommittedEvents = image.GetUncommittedEvents();
        
        if (uncommittedEvents.Count == 0)
            return;
        
        await _eventStore.AppendAsync(
            aggregateId: image.Id,
            expectedVersion: image.Version,
            events: uncommittedEvents.ToList(),
            cancellationToken: cancellationToken);
        
        image.UncommittedEventsClear();
    }
}