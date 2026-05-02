using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RenStore.Delivery.Application.Abstractions;
using RenStore.Delivery.Persistence.Outbox;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.EventStore;

internal sealed class SqlEventStore : IEventStore
{
    private readonly DeliveryDbContext _context;
    
    public SqlEventStore(DeliveryDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<IReadOnlyList<IDomainEvent>> LoadAsync(
        Guid aggregateId, 
        CancellationToken cancellationToken = default)
    {
        if (aggregateId == Guid.Empty)
        {
            throw new ArgumentException(
                "AggregateId cannot be empty.",
                nameof(aggregateId));
        }
        
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
        {
            throw new ArgumentException(
                "AggregateId cannot be empty.",
                nameof(aggregateId));
        }
        
        if (expectedVersion < 0)
        {
            throw new ArgumentException(
                "Expected version must be greater then or equal 0.",
                nameof(expectedVersion));
        }
        
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (events == null || events.Count == 0)  return;

        var version = expectedVersion;
        var now = DateTimeOffset.UtcNow;

        try
        {
            foreach (var domainEvent in events)
            {
                version++;
                
                var eventEntity = ToEventEntity(aggregateId, domainEvent, version);
                var outbox = ToOutboxMessageEntity(aggregateId, domainEvent, now);

                _context.Events.Add(eventEntity);
                _context.OutboxMessages.Add(outbox);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException e)
            when (IsUniqueViolation(e))
        {
            throw new ConcurrencyException(
                $"Concurrency conflict for aggregate {aggregateId}.", e);
        }
    }

    private EventEntity ToEventEntity(
        Guid aggregateId, 
        IDomainEvent domainEvent, 
        int version)
    {
        if (!DomainEventMappings.DomainEventsTypeToName
                .TryGetValue(domainEvent.GetType(), out var type))
        {
            throw new InvalidOperationException(
                $"Event type {domainEvent.GetType()} not registered at in " +
                $"{DomainEventMappings.DomainEventsTypeToName}.");
        }
            
        return new EventEntity()
        {
            Id            = domainEvent.EventId,
            AggregateId   = aggregateId,
            Version       = version,
            EventType     = type,
            AggregateType = "catalog", // TODO: временное решение
            Payload       = JsonSerializer.Serialize(
                domainEvent, 
                domainEvent.GetType(),
                EventSerializer.Options),
            OccurredAtUtc = domainEvent.OccurredAt
        };
    }

    private OutboxMessage ToOutboxMessageEntity(
        Guid aggregateId, 
        IDomainEvent domainEvent, 
        DateTimeOffset now)
    {
        if (!DomainEventMappings.DomainEventsTypeToName
                .TryGetValue(domainEvent.GetType(), out var type))
        {
            throw new InvalidOperationException(
                $"Event type {domainEvent.GetType()} not registered at in " +
                $"{DomainEventMappings.DomainEventsTypeToName}.");
        }
        
        return new OutboxMessage()
        {
            Id = domainEvent.EventId,
            EventType = type,
            AggregateId = aggregateId,
            Payload = JsonSerializer.Serialize(
                domainEvent, 
                domainEvent.GetType(),
                EventSerializer.Options),
            OccurredAt = domainEvent.OccurredAt,
            Kind = OutboxMessageKind.Domain,
            CreatedAt = now,
            RetryCount = 0
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
        
        var result = (IDomainEvent)JsonSerializer.Deserialize(
            eventEntity.Payload,
            type!,
            EventSerializer.Options)!;

        if (result is null)
            throw new InvalidOperationException(nameof(eventEntity));
        
        return result;
    }

    private static bool IsUniqueViolation(DbUpdateException e)
    {
        return e.InnerException is Npgsql.PostgresException pgEx &&
            pgEx.SqlState == Npgsql.PostgresErrorCodes.UniqueViolation;
    }
}