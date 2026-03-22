using RenStore.Catalog.Domain.Aggregates.Media.Events;

namespace RenStore.Catalog.Application.Features.VariantMedia.Notifications.DomainEvents;

internal sealed class VariantImageMarkedAsMainEventHandler
    : INotificationHandler<DomainEventNotification<ImageMainSetEvent>>
{
    private readonly IVariantImageProjection _variantImageProjection;

    public VariantImageMarkedAsMainEventHandler(
        IVariantImageProjection variantImageProjection)
    {
        _variantImageProjection = variantImageProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<ImageMainSetEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _variantImageProjection.MarkAsMain(
            now: notification.DomainEvent.OccurredAt,
            imageId: notification.DomainEvent.ImageId,
            cancellationToken: cancellationToken);

        await _variantImageProjection.SaveChangesAsync(cancellationToken);
    }
}