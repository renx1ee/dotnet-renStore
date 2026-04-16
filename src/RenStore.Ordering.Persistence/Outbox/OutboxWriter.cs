using System.Text.Json;
using RenStore.Order.Persistence.EventStore;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Order.Persistence.Outbox;

/// <summary>
/// Converts domain events raised by an aggregate into <see cref="OutboxMessage"/> records
/// and appends them to the DbContext change tracker.
/// Must be called before <c>SaveChangesAsync</c> so everything lands in one transaction.
/// </summary>
internal sealed class OutboxWriter
{
    /// <summary>
    /// Creates outbox messages for every domain event in <paramref name="events"/>
    /// and adds them to <paramref name="context"/>.
    /// </summary>
    public static void Write(
        OrderingDbContext context,
        Guid aggregateId,
        IEnumerable<IDomainEvent> events,
        DateTimeOffset now)
    {
        foreach (var domainEvent in events)
        {
            var eventType = domainEvent.GetType();
            var eventName = DomainEventMappings.GetEventName(eventType);

            // Serialize as the concrete type so all properties are captured
            var payload = JsonSerializer.Serialize(domainEvent, eventType, EventSerializer.Options);

            var message = new OutboxMessage
            {
                Id = domainEvent.EventId, // EventId == outbox record ID
                EventType = eventName,
                AggregateId = aggregateId,
                Payload = payload,
                OccurredAt = domainEvent.OccurredAt,
                CreatedAt = now,
                RetryCount = 0
            };

            context.OutboxMessages.Add(message);
        }
    }
}