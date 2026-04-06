using RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class AddedSizeToProductVariantEventHandler
    : INotificationHandler<DomainEventNotification<VariantSizeCreatedEvent>>
{
    private readonly IProductVariantSizeProjection _variantSizeProjection;

    public AddedSizeToProductVariantEventHandler(
        IProductVariantSizeProjection variantSizeProjection)
    {
        _variantSizeProjection = variantSizeProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantSizeCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _variantSizeProjection.AddAsync(
            new VariantSizeReadModel()
            {
                Id = notification.DomainEvent.SizeId,
                LetterSize = notification.DomainEvent.LetterSize,
                Type = notification.DomainEvent.SizeType,
                System = notification.DomainEvent.SizeSystem,
                IsDeleted = false,
                CreatedAt = notification.DomainEvent.OccurredAt,
                VariantId = notification.DomainEvent.VariantId
            }, 
            cancellationToken);

        await _variantSizeProjection.CommitAsync(cancellationToken);
    }
}