using MediatR;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Application.Common;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

public class ProductVariantRepository
    : RenStore.Catalog.Domain.Interfaces.Repository.IProductVariantRepository
{
    private readonly IEventStore _eventStore;
    private readonly IMediator _mediator;
    
    public ProductVariantRepository(
        IEventStore eventStore,
        IMediator mediator)
    { 
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _mediator = mediator     ?? throw new ArgumentNullException(nameof(mediator));
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

    public async Task SaveAsync(
        ProductVariant productVariant,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(productVariant);
        
        var uncommittedEvents = productVariant.GetUncommittedEvents();

        await _eventStore.AppendAsync(
            aggregateId: productVariant.Id,
            expectedVersion: productVariant.Version,
            events: uncommittedEvents.ToList(),
            cancellationToken: cancellationToken);

        foreach (var domainEvent in uncommittedEvents)
        {
            var notification = new DomainEventNotification<IDomainEvent>(domainEvent);
            await _mediator.Publish(notification, cancellationToken);
        }
        
        productVariant.UncommittedEventsClear();
    }
}