using Microsoft.IdentityModel.Tokens;
using RenStore.Inventory.Application.Abstractions.Projections;
using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Stock.Events;
using RenStore.Inventory.Domain.ReadModels;

namespace RenStore.Inventory.Application.Features.Stock.Notifications;

internal sealed class StockCreatedEventHandler
    : INotificationHandler<DomainEventNotification<StockCreatedEvent>>
{
    private readonly IStockProjection _stockProjection;

    public StockCreatedEventHandler(
        IStockProjection stockProjection)
    {
        _stockProjection = stockProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<StockCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        var stock = new VariantStockReadModel()
        {
            Id = notification.DomainEvent.StockId,
            CreatedAt = notification.DomainEvent.OccurredAt,
            SizeId = notification.DomainEvent.SizeId,
            VariantId = notification.DomainEvent.VariantId,
            InStock = notification.DomainEvent.InitialStock
        };

        await _stockProjection.AddAsync(
            stock: stock,
            cancellationToken: cancellationToken);

        await _stockProjection.SaveChangesAsync(cancellationToken);
    }
}