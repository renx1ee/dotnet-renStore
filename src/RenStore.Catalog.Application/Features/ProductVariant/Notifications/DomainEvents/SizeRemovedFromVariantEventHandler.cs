using RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class SizeRemovedFromVariantEventHandler
    : INotificationHandler<DomainEventNotification<VariantSizeRemovedEvent>>
{
    private readonly IProductVariantSizeProjection _sizeProjection;

    public SizeRemovedFromVariantEventHandler(
        IProductVariantSizeProjection sizeProjection)
    {
        _sizeProjection = sizeProjection;
    }
    // TODO:
    public async Task Handle(
        DomainEventNotification<VariantSizeRemovedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _sizeProjection.SoftDeleteAsync(
            variantId: notification.DomainEvent.VariantId,
            sizeId: notification.DomainEvent.SizeId,
            removedAt: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        await _sizeProjection.SaveChangesAsync(cancellationToken);
    }
}