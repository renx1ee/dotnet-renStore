using RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class AttributeValueUpdatedEventHandler
    : INotificationHandler<DomainEventNotification<AttributeValueUpdatedEvent>>
{
    private readonly IVariantAttributeProjection _attributeProjection;

    public AttributeValueUpdatedEventHandler(
        IVariantAttributeProjection attributeProjection)
    {
        _attributeProjection = attributeProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<AttributeValueUpdatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _attributeProjection.UpdateValueAsync(
            attributeId: notification.DomainEvent.AttributeId,
            value: notification.DomainEvent.Value,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}