using RenStore.Catalog.Domain.Aggregates.Product.Events;

namespace RenStore.Catalog.Application.Features.Product.Notifications.DomainEvents;

internal sealed class ProductHiddenEventHandler
    : INotificationHandler<DomainEventNotification<ProductHiddenEvent>>
{
    private readonly IProductProjection _productProjection;
    
    public ProductHiddenEventHandler(
        IProductProjection productProjection)
    {
        _productProjection = productProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<ProductHiddenEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _productProjection.HideAsync(
            productId: notification.DomainEvent.ProductId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        await _productProjection.CommitAsync(cancellationToken);
    }
}