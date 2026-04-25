using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class VariantDetailsTypeOfPackingUpdatedEventHandler
    : INotificationHandler<DomainEventNotification<VariantDetailsTypeOfPackingUpdatedEvent>>
{
    private readonly IVariantDetailProjection _detailProjection;

    public VariantDetailsTypeOfPackingUpdatedEventHandler(
        IVariantDetailProjection detailProjection)
    {
        _detailProjection = detailProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantDetailsTypeOfPackingUpdatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _detailProjection.DetailsTypeOfPackingUpdateAsync(
            now: notification.DomainEvent.OccurredAt,
            typeOfPacking: notification.DomainEvent.TypeOfPacking,
            detailsId: notification.DomainEvent.DetailId,
            cancellationToken: cancellationToken);
    }
}