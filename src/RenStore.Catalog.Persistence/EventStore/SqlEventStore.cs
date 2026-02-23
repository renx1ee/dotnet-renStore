using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Persistence.EventStore;

public class SqlEventStore 
    : RenStore.Catalog.Application.Abstractions.IEventStore
{
    private readonly CatalogDbContext _context;
    
    public SqlEventStore(CatalogDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<IReadOnlyList<IDomainEvent>> LoadAsync(
        Guid aggregateId, 
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Events
            .Where(x => x.AggregateId == aggregateId)
            .OrderBy(x => x.Version)
            .ToListAsync(cancellationToken);

        return entities.Select(ToDomainEvent).ToList();
    }
    
    // TODO:
    public async Task AppendAsync(
        Guid aggregateId, 
        int expectedVersion, 
        IReadOnlyList<IDomainEvent> events, 
        CancellationToken cancellationToken = default) 
    {
        
    }

    private EventEntity ToEntity(
        IDomainEvent domainEvent, 
        int version)
    {
        var key = DomainEventMappings.DomainEvents
            .FirstOrDefault(x => 
                x.Value == domainEvent.GetType()).Key; 
        
        if(key == null)
            throw new InvalidOperationException($"Event type {domainEvent.GetType()} not registered at in {DomainEventMappings.DomainEvents}.");
            
        return new EventEntity()
        {
            Id = domainEvent.EventId,
            Version = version,
            EventType = key,
            Data = JsonSerializer.Serialize(domainEvent),
            OccurredAtUtc = domainEvent.OccurredAt
        };
    }
    
    private IDomainEvent ToDomainEvent(EventEntity eventEntity)
    {
        if (!DomainEventMappings.DomainEvents.TryGetValue(eventEntity.EventType, out var type))
            throw new InvalidOperationException($"Unknown event type {eventEntity.EventType}");

        return (IDomainEvent)JsonSerializer.Deserialize(
            eventEntity.Data,
            type)!;
    }
}