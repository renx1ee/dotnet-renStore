using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class VariantDetailsCompositionUpdatedEventHandler
    : INotificationHandler<DomainEventNotification<VariantDetailsCompositionUpdatedEvent>>
{
    private readonly IVariantDetailProjection _detailProjection;

    public VariantDetailsCompositionUpdatedEventHandler(
        IVariantDetailProjection detailProjection)
    {
        _detailProjection = detailProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantDetailsCompositionUpdatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _detailProjection.DetailsCompositionUpdateAsync(
            now: notification.DomainEvent.OccurredAt,
            composition: notification.DomainEvent.Composition,
            detailsId: notification.DomainEvent.DetailId,
            cancellationToken: cancellationToken);

        await _detailProjection.CommitAsync(cancellationToken);
    }
}