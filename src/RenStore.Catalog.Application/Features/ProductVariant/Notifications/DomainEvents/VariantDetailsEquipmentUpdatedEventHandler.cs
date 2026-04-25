using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class VariantDetailsEquipmentUpdatedEventHandler
    : INotificationHandler<DomainEventNotification<VariantDetailsEquipmentUpdatedEvent>>
{
    private readonly IVariantDetailProjection _detailProjection;

    public VariantDetailsEquipmentUpdatedEventHandler(
        IVariantDetailProjection detailProjection)
    {
        _detailProjection = detailProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantDetailsEquipmentUpdatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _detailProjection.DetailsEquipmentUpdateAsync(
            now: notification.DomainEvent.OccurredAt,
            equipment: notification.DomainEvent.Equipment,
            detailsId: notification.DomainEvent.DetailId,
            cancellationToken: cancellationToken);
    }
}