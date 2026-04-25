using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class VariantRestoredEventHandler
    : INotificationHandler<DomainEventNotification<VariantRestoredEvent>>
{
    private readonly IProductVariantProjection _variantProjection;

    public VariantRestoredEventHandler(
        IProductVariantProjection variantProjection)
    {
        _variantProjection = variantProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantRestoredEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _variantProjection.RestoreAsync(
            variantId: notification.DomainEvent.VariantId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}