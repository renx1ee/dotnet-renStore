using RenStore.Catalog.Domain.Aggregates.Product.Events;

namespace RenStore.Catalog.Application.Features.Product.Notifications.DomainEvents;

internal sealed class ProductPublishedEventHandler
    : INotificationHandler<DomainEventNotification<ProductPublishedEvent>>
{
    private readonly IProductProjection _productProjection;

    public ProductPublishedEventHandler(
        IProductProjection productProjection)
    {
        _productProjection = productProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<ProductPublishedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _productProjection.PublishAsync(
            productId: notification.DomainEvent.ProductId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}