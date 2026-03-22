using RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class AttributeKeyUpdatedEventHandler
    : INotificationHandler<DomainEventNotification<AttributeKeyUpdatedEvent>>
{
    private readonly IVariantAttributeProjection _attributeProjection;

    public AttributeKeyUpdatedEventHandler(
        IVariantAttributeProjection attributeProjection)
    {
        _attributeProjection = attributeProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<AttributeKeyUpdatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _attributeProjection.UpdateKeyAsync(
            attributeId: notification.DomainEvent.AttributeId,
            key: notification.DomainEvent.Key,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        await _attributeProjection.SaveChangesAsync(cancellationToken);
    }
}