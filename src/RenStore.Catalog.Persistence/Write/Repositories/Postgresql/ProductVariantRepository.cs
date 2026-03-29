using MediatR;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Application.Common;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

internal sealed class ProductVariantRepository
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
    
    
    public async Task<IReadOnlyCollection<ProductVariant>> GetManyAsync(
        IReadOnlyCollection<Guid> ids, 
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(ids);

        if (ids.Count == 0) return [];

        var variants = new List<ProductVariant>();

        foreach (var id in ids)
        {
            var events = await _eventStore.LoadAsync(
                aggregateId: id,
                cancellationToken: cancellationToken);

            var variant = ProductVariant.Rehydrate(events);
            
            variants.Add(variant);
        }

        return variants;
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
            var notificationType = typeof(DomainEventNotification<>)
                .MakeGenericType(domainEvent.GetType());

            var notification = (INotification)Activator
                .CreateInstance(notificationType, domainEvent)!;
            
            await _mediator.Publish(notification, cancellationToken);
        }
        
        productVariant.UncommittedEventsClear();
    }

    public async Task SaveManyAsync(
        IReadOnlyCollection<ProductVariant> variants,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(variants);

        if (variants.Count == 0) return;

        var allUncommittedEvents = new List<(ProductVariant variant, IReadOnlyCollection<IDomainEvent> events)>();
        
        foreach (var variant in variants)
        {
            var uncommittedEvents = variant.GetUncommittedEvents();
            
            await _eventStore.AppendAsync(
                aggregateId: variant.Id,
                expectedVersion: variant.Version,
                events: uncommittedEvents.ToList(),
                cancellationToken: cancellationToken);
        
            allUncommittedEvents.Add((variant, uncommittedEvents));
        }

        foreach (var (variant, events) in allUncommittedEvents)
        {
            foreach (var domainEvent in events)
            {
                var notificationType = typeof(DomainEventNotification<>)
                    .MakeGenericType(domainEvent.GetType());

                var notification = (INotification)Activator
                    .CreateInstance(notificationType, domainEvent)!;
            
                await _mediator.Publish(notification, cancellationToken);
            }
        
            variant.UncommittedEventsClear();
        }
    }
}