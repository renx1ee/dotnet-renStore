using RenStore.Catalog.Domain.Aggregates.Product.Events;

namespace RenStore.Catalog.Application.Features.Product.Notifications.DomainEvents;

internal sealed class ProductApproveEventHandler
    : INotificationHandler<DomainEventNotification<ProductApprovedEvent>>
{
    private readonly IProductProjection _productProjection;

    public ProductApproveEventHandler(
        IProductProjection productProjection)
    {
        _productProjection = productProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<ProductApprovedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _productProjection.ApproveAsync(
            productId: notification.DomainEvent.ProductId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);
    }
}