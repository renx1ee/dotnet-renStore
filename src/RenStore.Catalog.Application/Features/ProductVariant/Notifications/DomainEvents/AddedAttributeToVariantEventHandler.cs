using RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;
using RenStore.Catalog.Domain.ValueObjects;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class AddedAttributeToVariantEventHandler
    : INotificationHandler<DomainEventNotification<AttributeCreatedEvent>>
{
    private readonly IVariantAttributeProjection _attributeProjection;

    public AddedAttributeToVariantEventHandler(
        IVariantAttributeProjection attributeProjection)
    {
        _attributeProjection = attributeProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<AttributeCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _attributeProjection.AddAsync(
            attribute: new VariantAttributeReadModel()
            {
                Id = notification.DomainEvent.AttributeId,
                Key = AttributeKey.Create(notification.DomainEvent.Key),
                Value = AttributeValue.Create(notification.DomainEvent.Value),
                IsDeleted = false,
                CreatedAt = notification.DomainEvent.OccurredAt,
                VariantId = notification.DomainEvent.VariantId
            }, 
            cancellationToken: cancellationToken);

        await _attributeProjection.CommitAsync(cancellationToken);
    }
}