using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Stock.Events;

namespace RenStore.Inventory.Application.Features.Stock.Notifications;

internal sealed class StockAddedEventHandler
    : INotificationHandler<DomainEventNotification<StockAddedEvent>>
{
    private readonly IStockProjection _stockProjection;
    
    public StockAddedEventHandler(
        IStockProjection stockProjection)
    {
        _stockProjection = stockProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<StockAddedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _stockProjection.AddToStockAsync(
            now: notification.DomainEvent.OccurredAt,
            stockId: notification.DomainEvent.StockId,
            count: notification.DomainEvent.Count,
            cancellationToken: cancellationToken);

        await _stockProjection.SaveChangesAsync(cancellationToken);
    }
}