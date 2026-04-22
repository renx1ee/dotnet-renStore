using System.Text.Json;
using RenStore.Order.Application.Abstractions;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Order.Persistence.Outbox;

internal sealed class IntegrationOutboxWriter : IIntegrationOutboxWriter
{
    private readonly OrderingDbContext _context;

    public IntegrationOutboxWriter(OrderingDbContext context) => 
        _context = context;

    public void Stage<T>(T integrationEvent) where T : IIntegrationEvent
    {
        var type = integrationEvent.GetType();

        _context.OutboxMessages.Add(new OutboxMessage
        {
            Id        = Guid.NewGuid(),
            EventType = IntegrationEventMappings.GetEventName(type),
            Payload   = JsonSerializer.Serialize(
                integrationEvent, type, EventSerializer.Options),
            Kind      = OutboxMessageKind.Integration,
            CreatedAt = DateTimeOffset.UtcNow,
        });
    }
}