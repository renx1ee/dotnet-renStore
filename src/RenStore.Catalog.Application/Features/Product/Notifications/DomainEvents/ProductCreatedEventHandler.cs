using RenStore.Catalog.Domain.Aggregates.Product.Events;

namespace RenStore.Catalog.Application.Features.Product.Notifications.DomainEvents;

internal sealed class ProductCreatedEventHandler
    : INotificationHandler<DomainEventNotification<ProductCreatedEvent>>
{
    private readonly IProductProjection _productProjection;

    public ProductCreatedEventHandler(
        IProductProjection productProjection)
    {
        _productProjection = productProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<ProductCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        var product = new ProductReadModel()
        {
            Id = notification.DomainEvent.ProductId,
            SellerId = notification.DomainEvent.SellerId,
            SubCategoryId = notification.DomainEvent.SubCategoryId,
            CategoryId = notification.DomainEvent.CategoryId,
            Status = notification.DomainEvent.Status,
            CreatedAt = notification.DomainEvent.OccurredAt,
        };
        
        await _productProjection.AddAsync(product, cancellationToken);
        await _productProjection.SaveChangesAsync(cancellationToken);
    }
}