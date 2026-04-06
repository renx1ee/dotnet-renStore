using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class SoftDeletedVariantEventHandler
    : INotificationHandler<DomainEventNotification<VariantRemovedEvent>>
{
    private readonly IProductVariantProjection _variantProjection;

    public SoftDeletedVariantEventHandler(
        IProductVariantProjection variantProjection)
    {
        _variantProjection = variantProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantRemovedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _variantProjection.SoftDeleteAsync(
            variantId: notification.DomainEvent.VariantId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        await _variantProjection.CommitAsync(cancellationToken);
    }
}