using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class VariantDetailsDescriptionUpdatedEventHandler
    : INotificationHandler<DomainEventNotification<VariantDetailsDescriptionUpdatedEvent>>
{
    private readonly IVariantDetailProjection _detailProjection;

    public VariantDetailsDescriptionUpdatedEventHandler(
        IVariantDetailProjection detailProjection)
    {
        _detailProjection = detailProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantDetailsDescriptionUpdatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _detailProjection.DetailsDescriptionUpdateAsync(
            now: notification.DomainEvent.OccurredAt,
            description: notification.DomainEvent.Description,
            detailsId: notification.DomainEvent.DetailId,
            cancellationToken: cancellationToken);
    }
}