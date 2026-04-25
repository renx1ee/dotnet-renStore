using RenStore.Catalog.Domain.Aggregates.Product.Events;

namespace RenStore.Catalog.Application.Features.Product.Notifications.DomainEvents;

internal sealed class ProductDraftedEventHandler
    : INotificationHandler<DomainEventNotification<ProductMovedToDraftEvent>>
{
    private readonly IProductProjection _productProjection;

    public ProductDraftedEventHandler(
        IProductProjection productProjection)
    {
        _productProjection = productProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<ProductMovedToDraftEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _productProjection.DraftAsync(
            productId: notification.DomainEvent.ProductId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}