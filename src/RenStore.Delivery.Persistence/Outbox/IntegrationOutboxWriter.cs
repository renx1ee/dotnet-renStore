using System.Text.Json;
using RenStore.Delivery.Application.Abstractions;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Delivery.Persistence.Outbox;

internal sealed class IntegrationOutboxWriter : IIntegrationOutboxWriter
{
    private readonly DeliveryDbContext _context;

    public IntegrationOutboxWriter(DeliveryDbContext context) => 
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
            CreatedAt = DateTimeOffset.UtcNow
        });
    }
}