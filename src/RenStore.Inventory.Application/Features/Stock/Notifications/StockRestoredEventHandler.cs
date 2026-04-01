using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Stock.Events;

namespace RenStore.Inventory.Application.Features.Stock.Notifications;

internal sealed class StockRestoredEventHandler
    : INotificationHandler<DomainEventNotification<StockRestoredEvent>>
{
    private readonly IStockProjection _stockProjection;
    
    public StockRestoredEventHandler(
        IStockProjection stockProjection)
    {
        _stockProjection = stockProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<StockRestoredEvent> notification,
        CancellationToken cancellationToken)
    {
        await _stockProjection.Restore(
            now: notification.DomainEvent.OccurredAt,
            stockId: notification.DomainEvent.StockId,
            cancellationToken: cancellationToken);

        await _stockProjection.SaveChangesAsync(cancellationToken);
    }
}