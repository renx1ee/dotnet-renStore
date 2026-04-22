using RenStore.Catalog.Contracts.Events;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class AddedSizeToProductVariantEventHandler
    : INotificationHandler<DomainEventNotification<VariantSizeCreatedEvent>>
{
    private readonly IProductVariantSizeProjection _variantSizeProjection;
    private readonly IIntegrationOutboxWriter _outboxWriter;

    public AddedSizeToProductVariantEventHandler(
        IProductVariantSizeProjection variantSizeProjection,
        IIntegrationOutboxWriter outboxWriter)
    {
        _variantSizeProjection = variantSizeProjection;
        _outboxWriter          = outboxWriter;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantSizeCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _variantSizeProjection.AddAsync(
            new VariantSizeReadModel()
            {
                Id         = notification.DomainEvent.SizeId,
                LetterSize = notification.DomainEvent.LetterSize,
                Type       = notification.DomainEvent.SizeType,
                System     = notification.DomainEvent.SizeSystem,
                IsDeleted  = false,
                CreatedAt  = notification.DomainEvent.OccurredAt,
                VariantId  = notification.DomainEvent.VariantId
            }, cancellationToken);

        _outboxWriter.Stage(new VariantSizeCreatedIntegrationEvent(
            VariantId: notification.DomainEvent.VariantId,
            SizeId:    notification.DomainEvent.SizeId));
    }
}