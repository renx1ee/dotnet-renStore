using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class VariantDetailsCaringOfThingsUpdatedEventHandler
    : INotificationHandler<DomainEventNotification<VariantDetailsCaringOfThingsUpdatedEvent>>
{
    private readonly IVariantDetailProjection _detailProjection;

    public VariantDetailsCaringOfThingsUpdatedEventHandler(
        IVariantDetailProjection detailProjection)
    {
        _detailProjection = detailProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantDetailsCaringOfThingsUpdatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _detailProjection.DetailsCaringOfThingsUpdateAsync(
            now: notification.DomainEvent.OccurredAt,
            caringOfThings: notification.DomainEvent.CaringOfThings,
            detailsId: notification.DomainEvent.DetailId,
            cancellationToken: cancellationToken);

        await _detailProjection.CommitAsync(cancellationToken);
    }
}