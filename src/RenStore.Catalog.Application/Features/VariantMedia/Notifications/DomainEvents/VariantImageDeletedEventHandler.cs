using RenStore.Catalog.Domain.Aggregates.Media.Events;

namespace RenStore.Catalog.Application.Features.VariantMedia.Notifications.DomainEvents;

internal sealed class VariantImageDeletedEventHandler
    : INotificationHandler<DomainEventNotification<VariantImageRemovedEvent>>
{
    private readonly IVariantImageProjection _variantImageProjection;
    
    public VariantImageDeletedEventHandler(
        IVariantImageProjection variantImageProjection)
    {
        _variantImageProjection = variantImageProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantImageRemovedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _variantImageProjection.SoftDelete(
            notification.DomainEvent.OccurredAt,
            imageId: notification.DomainEvent.ImageId,
            cancellationToken: cancellationToken);
    }
}