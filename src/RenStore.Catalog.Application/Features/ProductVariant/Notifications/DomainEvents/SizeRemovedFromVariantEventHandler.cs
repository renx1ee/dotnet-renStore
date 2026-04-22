using RenStore.Catalog.Contracts.Events;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class SizeRemovedFromVariantEventHandler
    : INotificationHandler<DomainEventNotification<VariantSizeRemovedEvent>>
{
    private readonly IProductVariantSizeProjection _sizeProjection;
    private readonly IIntegrationOutboxWriter _outboxWriter;

    public SizeRemovedFromVariantEventHandler(
        IProductVariantSizeProjection sizeProjection,
        IIntegrationOutboxWriter outboxWriter)
    {
        _sizeProjection = sizeProjection;
        _outboxWriter   = outboxWriter;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantSizeRemovedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _sizeProjection.SoftDeleteAsync(
            variantId: notification.DomainEvent.VariantId,
            sizeId: notification.DomainEvent.SizeId,
            removedAt: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        _outboxWriter.Stage(new VariantSizeDeletedIntegrationEvent(
            VariantId: notification.DomainEvent.VariantId,
            SizeId: notification.DomainEvent.SizeId));
    }
}