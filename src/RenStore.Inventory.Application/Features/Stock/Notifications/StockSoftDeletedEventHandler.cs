using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Stock.Events;

namespace RenStore.Inventory.Application.Features.Stock.Notifications;

internal sealed class StockSoftDeletedEventHandler
    : INotificationHandler<DomainEventNotification<StockSoftDeletedEvent>>
{
    private readonly IStockProjection _stockProjection;
    
    public StockSoftDeletedEventHandler(
        IStockProjection stockProjection)
    {
        _stockProjection = stockProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<StockSoftDeletedEvent> notification,
        CancellationToken cancellationToken)
    {
        await _stockProjection.SoftDelete(
            now: notification.DomainEvent.OccurredAt,
            stockId: notification.DomainEvent.StockId,
            cancellationToken: cancellationToken);

        await _stockProjection.SaveChangesAsync(cancellationToken);
    }
}