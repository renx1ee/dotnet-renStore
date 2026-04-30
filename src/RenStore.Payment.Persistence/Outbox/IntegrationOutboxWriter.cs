using System.Text.Json;
using RenStore.Payment.Application.Abstractions;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Payment.Persistence.Outbox;

public class IntegrationOutboxWriter : IIntegrationOutboxWriter
{
    private readonly PaymentDbContext _context;

    public IntegrationOutboxWriter(PaymentDbContext context) => 
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