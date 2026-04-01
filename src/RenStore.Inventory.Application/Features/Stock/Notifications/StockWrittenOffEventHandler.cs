using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Stock.Events;

namespace RenStore.Inventory.Application.Features.Stock.Notifications;

internal sealed class StockWrittenOffEventHandler
    : INotificationHandler<DomainEventNotification<StockWrittenOffEvent>>
{
    private readonly IStockProjection _stockProjection;
    
    public StockWrittenOffEventHandler(
        IStockProjection stockProjection)
    {
        _stockProjection = stockProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<StockWrittenOffEvent> notification, 
        CancellationToken cancellationToken)
    {
        await _stockProjection.StockWriteOffAsync(
            now: notification.DomainEvent.OccurredAt,
            stockId: notification.DomainEvent.StockId,
            count: notification.DomainEvent.Count,
            reason: notification.DomainEvent.Reason,
            cancellationToken: cancellationToken);

        await _stockProjection.SaveChangesAsync(cancellationToken);
    }
}