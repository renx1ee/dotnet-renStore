using RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class AttributeRestoredEventHandler
    : INotificationHandler<DomainEventNotification<AttributeRestoredEvent>>
{
    private readonly IVariantAttributeProjection _attributeProjection;

    public AttributeRestoredEventHandler(
        IVariantAttributeProjection attributeProjection)
    {
        _attributeProjection = attributeProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<AttributeRestoredEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _attributeProjection.RestoreAsync(
            attributeId: notification.DomainEvent.AttributeId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        await _attributeProjection.SaveChangesAsync(cancellationToken);
    }
}