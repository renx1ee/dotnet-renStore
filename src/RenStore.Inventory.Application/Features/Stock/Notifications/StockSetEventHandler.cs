using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Stock.Events;

namespace RenStore.Inventory.Application.Features.Stock.Notifications;

internal sealed class StockSetEventHandler
    : INotificationHandler<DomainEventNotification<StockSetEvent>>
{
    private readonly IStockProjection _stockProjection;
    
    public StockSetEventHandler(
        IStockProjection stockProjection)
    {
        _stockProjection = stockProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<StockSetEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _stockProjection.SetStockAsync(
            now: notification.DomainEvent.OccurredAt,
            stockId: notification.DomainEvent.StockId,
            count: notification.DomainEvent.NewStock,
            cancellationToken: cancellationToken);

        await _stockProjection.SaveChangesAsync(cancellationToken);
    }
}