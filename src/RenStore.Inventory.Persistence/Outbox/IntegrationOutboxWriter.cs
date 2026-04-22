using System.Text.Json;
using RenStore.Inventory.Application.Abstractions;
using RenStore.Inventory.Persistence.EventStore;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Persistence.Outbox;

internal sealed class IntegrationOutboxWriter : IIntegrationOutboxWriter
{
    private readonly InventoryDbContext _context;

    public IntegrationOutboxWriter(InventoryDbContext context) => 
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