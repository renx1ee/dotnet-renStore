using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.VariantArchived;

internal sealed class ArchivedProductVariantEventHandler
    : INotificationHandler<DomainEventNotification<VariantArchivedEvent>>
{
    private readonly IProductVariantProjection _variantProjection;

    public ArchivedProductVariantEventHandler(
        IProductVariantProjection variantProjection)
    {
        _variantProjection = variantProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantArchivedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _variantProjection.ArchiveAsync(
            variantId: notification.DomainEvent.VariantId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        await _variantProjection.SaveChangesAsync(cancellationToken);
    }
}