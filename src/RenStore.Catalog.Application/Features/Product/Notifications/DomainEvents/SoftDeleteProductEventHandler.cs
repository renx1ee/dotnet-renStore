using RenStore.Catalog.Domain.Aggregates.Product.Events;

namespace RenStore.Catalog.Application.Features.Product.Notifications.DomainEvents;

internal sealed class SoftDeleteProductEventHandler
    : INotificationHandler<DomainEventNotification<ProductRemovedEvent>>
{
    private readonly IProductProjection _productProjection;

    public SoftDeleteProductEventHandler(
        IProductProjection productProjection)
    {
        _productProjection = productProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<ProductRemovedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _productProjection.SoftDeleteAsync(
            productId: notification.DomainEvent.ProductId,
            now: notification.DomainEvent.OccurredAt, 
            cancellationToken: cancellationToken);

        await _productProjection.CommitAsync(cancellationToken);
    }
}