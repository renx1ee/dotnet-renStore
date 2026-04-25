using RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class AttributeRemovedEventHandler
    : INotificationHandler<DomainEventNotification<AttributeRemovedEvent>>
{
    private readonly IVariantAttributeProjection _attributeProjection;

    public AttributeRemovedEventHandler(
        IVariantAttributeProjection attributeProjection)
    {
        _attributeProjection = attributeProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<AttributeRemovedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _attributeProjection.SoftDeleteAsync(
            attributeId: notification.DomainEvent.AttributeId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}