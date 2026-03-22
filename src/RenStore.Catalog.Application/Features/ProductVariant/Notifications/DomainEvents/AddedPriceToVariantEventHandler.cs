using RenStore.Catalog.Domain.Aggregates.Variant.Events.Price;

namespace RenStore.Catalog.Application.Features.ProductVariant.Notifications.DomainEvents;

internal sealed class AddedPriceToVariantEventHandler
    : INotificationHandler<DomainEventNotification<PriceCreatedEvent>>
{
    private readonly ISizePriceProjection _priceProjection; 
    
    public AddedPriceToVariantEventHandler(
        ISizePriceProjection priceProjection)
    {
        _priceProjection = priceProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<PriceCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        var price = new PriceHistoryReadModel()
        {
            Id = notification.DomainEvent.PriceId,
            Amount = notification.DomainEvent.PriceAmount,
            Currency = notification.DomainEvent.Currency,
            ValidFrom = notification.DomainEvent.EffectiveFrom,
            IsActive = true,
            CreatedAt = notification.DomainEvent.OccurredAt,
            DeactivatedAt = null,
            SizeId = notification.DomainEvent.SizeId
        };

        await _priceProjection
            .AddAsync(price, cancellationToken);

        await _priceProjection.SaveChangesAsync(cancellationToken);
    }
}