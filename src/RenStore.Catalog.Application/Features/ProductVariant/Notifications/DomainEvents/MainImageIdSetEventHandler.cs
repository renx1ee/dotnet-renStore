using RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class MainImageIdSetEventHandler
    : INotificationHandler<DomainEventNotification<MainImageIdSetEvent>>
{
    private readonly IProductVariantProjection _variantProjection;

    public MainImageIdSetEventHandler(
        IProductVariantProjection variantProjection)
    {
        _variantProjection = variantProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<MainImageIdSetEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _variantProjection.SetMainImageIdAsyncAsync(
            variantId: notification.DomainEvent.VariantId,
            imageId: notification.DomainEvent.ImageId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        await _variantProjection.SaveChangesAsync(cancellationToken);
    }
}