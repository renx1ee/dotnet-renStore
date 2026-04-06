using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class VariantPublishedEventHandler
    : INotificationHandler<DomainEventNotification<VariantPublishedEvent>>
{
    private readonly IProductVariantProjection _variantProjection;
    
    public VariantPublishedEventHandler(
        IProductVariantProjection variantProjection)
    {
        _variantProjection = variantProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantPublishedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _variantProjection.PublishAsync(
            variantId: notification.DomainEvent.VariantId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        await _variantProjection.CommitAsync(cancellationToken);
    }
}