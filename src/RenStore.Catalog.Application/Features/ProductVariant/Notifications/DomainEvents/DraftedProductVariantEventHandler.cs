using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class DraftedProductVariantEventHandler
    : INotificationHandler<DomainEventNotification<VariantDraftedEvent>>
{
    private readonly IProductVariantProjection _variantProjection;

    public DraftedProductVariantEventHandler(
        IProductVariantProjection variantProjection)
    {
        _variantProjection = variantProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantDraftedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _variantProjection.DraftAsync(
            variantId: notification.DomainEvent.VariantId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}