using RenStore.Catalog.Application.Common;
using RenStore.Catalog.Domain.Aggregates.Product.Events;

namespace RenStore.Catalog.Application.Features.Product.Notifications.Reject;

internal sealed class ProductRejectedEventHandler
    : INotificationHandler<DomainEventNotification<ProductRejectedEvent>>
{
    private readonly IProductProjection _productProjection;

    public ProductRejectedEventHandler(
        IProductProjection productProjection)
    {
        _productProjection = productProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<ProductRejectedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _productProjection.RejectAsync(
            productId: notification.DomainEvent.ProductId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        await _productProjection.SaveChangesAsync(cancellationToken);
    }
}