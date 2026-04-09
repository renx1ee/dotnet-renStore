using MediatR;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Media;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

internal sealed class VariantImageRepository
    : RenStore.Catalog.Domain.Interfaces.Repository.IVariantImageRepository   
{
    private readonly IEventStore _eventStore;
    private readonly IMediator _mediator;
    
    public VariantImageRepository(
        IEventStore eventStore,
        IMediator mediator)
    { 
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _mediator = mediator     ?? throw new ArgumentNullException(nameof(mediator));
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

        await _eventStore.AppendAsync(
            aggregateId: image.Id,
            expectedVersion: image.Version,
            events: uncommittedEvents.ToList(),
            cancellationToken: cancellationToken);

        foreach (var domainEvent in uncommittedEvents)
        {
            var notificationType = typeof(DomainEventNotification<>)
                .MakeGenericType(domainEvent.GetType());

            var notification = (INotification)Activator
                .CreateInstance(notificationType, domainEvent)!;
            
            await _mediator.Publish(notification, cancellationToken);
        }
        
        image.UncommittedEventsClear();
    }
}