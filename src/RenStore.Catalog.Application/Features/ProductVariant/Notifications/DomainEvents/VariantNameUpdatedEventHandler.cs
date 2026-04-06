using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class VariantNameUpdatedEventHandler
    : INotificationHandler<DomainEventNotification<VariantNameUpdatedEvent>>
{
    private readonly IProductVariantProjection _variantProjection;

    public VariantNameUpdatedEventHandler(
        IProductVariantProjection variantProjection)
    {
        _variantProjection = variantProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantNameUpdatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _variantProjection
            .ChangeNameAsync(
                variantId: notification.DomainEvent.VariantId,
                name: notification.DomainEvent.Name,
                normalizedName: notification.DomainEvent.NormalizedName,
                now: notification.DomainEvent.OccurredAt,
                cancellationToken: cancellationToken);

        await _variantProjection.CommitAsync(cancellationToken);
    }
}