using System.Text.Json;
using RenStore.Identity.Application.Abstractions;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Persistence.Outbox;

internal sealed class IntegrationOutboxWriter : IIntegrationOutboxWriter
{
    private readonly IdentityDbContext _context;

    public IntegrationOutboxWriter(IdentityDbContext context) => 
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