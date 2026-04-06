using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class VariantDetailsModelFeaturesUpdatedEventHandler
    : INotificationHandler<DomainEventNotification<VariantDetailsModelFeaturesUpdatedEvent>>
{
    private readonly IVariantDetailProjection _detailProjection;

    public VariantDetailsModelFeaturesUpdatedEventHandler(
        IVariantDetailProjection detailProjection)
    {
        _detailProjection = detailProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantDetailsModelFeaturesUpdatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _detailProjection.DetailsModelFeaturesUpdateAsync(
            now: notification.DomainEvent.OccurredAt,
            modelFeatures: notification.DomainEvent.ModelFeatures,
            detailsId: notification.DomainEvent.DetailId,
            cancellationToken: cancellationToken);

        await _detailProjection.CommitAsync(cancellationToken);
    }
}