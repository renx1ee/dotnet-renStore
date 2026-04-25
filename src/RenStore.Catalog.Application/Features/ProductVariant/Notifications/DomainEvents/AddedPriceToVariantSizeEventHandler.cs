using RenStore.Catalog.Domain.Aggregates.Variant.Events.Price;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class AddedPriceToVariantSizeEventHandler
    : INotificationHandler<DomainEventNotification<PriceCreatedEvent>>
{
    private readonly ISizePriceProjection _sizePriceProjection;

    public AddedPriceToVariantSizeEventHandler(
        ISizePriceProjection sizePriceProjection)
    {
        _sizePriceProjection = sizePriceProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<PriceCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _sizePriceProjection.AddAsync(
            price: new PriceHistoryReadModel()
            {
                Id = notification.DomainEvent.PriceId,
                Amount = notification.DomainEvent.PriceAmount,
                Currency = notification.DomainEvent.Currency,
                ValidFrom = notification.DomainEvent.EffectiveFrom,
                CreatedAt = notification.DomainEvent.OccurredAt,
                IsActive = true,
                SizeId = notification.DomainEvent.SizeId
            },
            cancellationToken: cancellationToken);
    }
}