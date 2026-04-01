using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Stock.Events;

namespace RenStore.Inventory.Application.Features.Stock.Notifications;

internal sealed class StockSoldEventHandler
    : INotificationHandler<DomainEventNotification<StockSoldEvent>>
{
    private readonly IStockProjection _stockProjection;
    
    public StockSoldEventHandler(
        IStockProjection stockProjection)
    {
        _stockProjection = stockProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<StockSoldEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _stockProjection.SellAsync(
            now: notification.DomainEvent.OccurredAt,
            stockId: notification.DomainEvent.StockId,
            count: notification.DomainEvent.Count,
            cancellationToken: cancellationToken);

        await _stockProjection.SaveChangesAsync(cancellationToken);
    }
}