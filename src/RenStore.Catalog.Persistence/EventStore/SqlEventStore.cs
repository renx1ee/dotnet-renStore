using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Exceptions;

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
        if (aggregateId == Guid.Empty)
            throw new InvalidOperationException(nameof(aggregateId));
        
        var entities = await _context.Events
            .Where(x => x.AggregateId == aggregateId)
            .OrderBy(x => x.Version)
            .ToListAsync(cancellationToken);
        
        return entities.Select(ToDomainEvent).ToList();
    }
    
    public async Task AppendAsync(
        Guid aggregateId, 
        int expectedVersion, 
        IReadOnlyList<IDomainEvent> events, 
        CancellationToken cancellationToken = default) 
    {
        if (aggregateId == Guid.Empty)
            throw new InvalidOperationException(nameof(aggregateId));
        
        if (expectedVersion < 0)
            throw new InvalidOperationException(nameof(expectedVersion));
        
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (events == null || events.Count == 0)
            return;

        var version = expectedVersion;

        foreach (var domainEvent in events)
        {
            version++;
            
            var eventEntity = ToEntity(aggregateId, domainEvent, version);
            
            _context.Events.Add(eventEntity);
        }

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e) 
            when(IsUniqueViolation(e)) 
        { 
            throw new ConcurrencyException(
                $"Concurrency conflict for aggregate {aggregateId}.", e);
        }
    }

    private EventEntity ToEntity(
        Guid aggregateId, 
        IDomainEvent domainEvent, 
        int version)
    {
        if (!DomainEventMappings.DomainEventsTypeToName
                .TryGetValue(domainEvent.GetType(), out var type))
        {
            throw new InvalidOperationException(
                $"Event type {domainEvent.GetType()} not registered at in {DomainEventMappings.DomainEventsTypeToName}.");
        }
            
        return new EventEntity()
        {
            Id = domainEvent.EventId,
            AggregateId = aggregateId,
            Version = version,
            EventType = type,
            AggregateType = "catalog", // TODO: временное решение
            Payload = JsonSerializer.Serialize(
                domainEvent, 
                domainEvent.GetType(),
                EventSerializer.Options),
            OccurredAtUtc = domainEvent.OccurredAt
        };
    }
    
    private IDomainEvent ToDomainEvent(EventEntity eventEntity)
    {
        if (!DomainEventMappings.DomainEventsNameToType
                .TryGetValue(eventEntity.EventType, out var type))
        {
            throw new InvalidOperationException(  
                $"Unknown event type {eventEntity.EventType}");
        }
        
        return (IDomainEvent)JsonSerializer.Deserialize(
            eventEntity.Payload,
            type!,
            EventSerializer.Options)!;
    }

    private static bool IsUniqueViolation(DbUpdateException e)
    {
        return e.InnerException is Npgsql.PostgresException pgEx &&
            pgEx.SqlState == Npgsql.PostgresErrorCodes.UniqueViolation;
    }
}