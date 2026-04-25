using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class VariantDetailsCreatedEventHandler
    : INotificationHandler<DomainEventNotification<VariantDetailsCreatedEvent>>
{
    private readonly IVariantDetailProjection _detailProjection;

    public VariantDetailsCreatedEventHandler(
        IVariantDetailProjection detailProjection)
    {
        _detailProjection = detailProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantDetailsCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _detailProjection.DetailsCompositionUpdateAsync(
            now: notification.DomainEvent.OccurredAt,
            composition: notification.DomainEvent.Composition,
            detailsId: notification.DomainEvent.DetailId,
            cancellationToken: cancellationToken);
    }
}