using MediatR;
using RenStore.Catalog.Application.Abstractions.Projections;
using RenStore.Catalog.Application.Common;
using RenStore.Catalog.Domain.Aggregates.Product.Events;

namespace RenStore.Catalog.Application.Features.Product.Notifications.Archive;

internal sealed class ProductArchivedEventHandler
    : INotificationHandler<DomainEventNotification<ProductArchivedEvent>>
{
    private readonly IProductProjection _productProjection;

    public ProductArchivedEventHandler(
        IProductProjection productProjection)
    {
        _productProjection = productProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<ProductArchivedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _productProjection.RejectAsync(
            productId: notification.DomainEvent.ProductId,
            now: notification.DomainEvent.OccurredAt,
            cancellationToken: cancellationToken);

        await _productProjection.SaveChangesAsync(cancellationToken);
    }
}