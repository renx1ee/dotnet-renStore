using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class VariantDetailsDecorativeElementsUpdatedEventHandler
    : INotificationHandler<DomainEventNotification<VariantDetailsDecorativeElementsUpdatedEvent>>
{
    private readonly IVariantDetailProjection _detailProjection;

    public VariantDetailsDecorativeElementsUpdatedEventHandler(
        IVariantDetailProjection detailProjection)
    {
        _detailProjection = detailProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantDetailsDecorativeElementsUpdatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _detailProjection.DetailsDecorativeElementsUpdateAsync(
            now: notification.DomainEvent.OccurredAt,
            decorativeElements: notification.DomainEvent.DecorativeElements,
            detailsId: notification.DomainEvent.DetailId,
            cancellationToken: cancellationToken);

        await _detailProjection.CommitAsync(cancellationToken);
    }
}