using RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

public class VariantSizeRestoredEventHandler
    : INotificationHandler<DomainEventNotification<VariantSizeRestoredEvent>>
{
    private readonly IProductVariantSizeProjection _variantSizeProjection;
    
    public VariantSizeRestoredEventHandler(
        IProductVariantSizeProjection variantSizeProjection)
    {
        _variantSizeProjection = variantSizeProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantSizeRestoredEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _variantSizeProjection.RestoreAsync(
            variantId: notification.DomainEvent.VariantId,
            sizeId: notification.DomainEvent.SizeId,
            restoredAt: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}