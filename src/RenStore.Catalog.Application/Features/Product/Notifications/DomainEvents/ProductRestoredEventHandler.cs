using RenStore.Catalog.Domain.Aggregates.Product.Events;

namespace RenStore.Catalog.Application.Features.Product.Notifications.DomainEvents;

internal sealed class ProductRestoredEventHandler
    : INotificationHandler<DomainEventNotification<ProductRestoredEvent>>
{
    private readonly IProductProjection _productProjection;
    
    public ProductRestoredEventHandler(
        IProductProjection productProjection)
    {
        _productProjection = productProjection;
    }

    public async Task Handle(
        DomainEventNotification<ProductRestoredEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _productProjection
            .RestoreAsync(
                notification.DomainEvent.ProductId,
                notification.DomainEvent.OccurredAt,
                cancellationToken: cancellationToken);

        await _productProjection.SaveChangesAsync(cancellationToken);
    }
}