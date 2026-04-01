using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Stock.Events;

namespace RenStore.Inventory.Application.Features.Stock.Notifications;

internal sealed class StockSaleReturnedEventHandler
    : INotificationHandler<DomainEventNotification<StockSaleReturnedEvent>>
{
    private readonly IStockProjection _stockProjection;
    
    public StockSaleReturnedEventHandler(
        IStockProjection stockProjection)
    {
        _stockProjection = stockProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<StockSaleReturnedEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _stockProjection.ReturnSaleAsync(
            now: notification.DomainEvent.OccurredAt,
            stockId: notification.DomainEvent.StockId,
            count: notification.DomainEvent.Count,
            cancellationToken: cancellationToken);

        await _stockProjection.SaveChangesAsync(cancellationToken);
    }
}