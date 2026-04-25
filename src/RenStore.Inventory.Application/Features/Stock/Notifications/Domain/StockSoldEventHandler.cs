using RenStore.Inventory.Application.Abstractions.ReadRepository;
using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Contracts.Events;
using RenStore.Inventory.Domain.Aggregates.Stock.Events;
using RenStore.Inventory.Domain.ReadModels;

namespace RenStore.Inventory.Application.Features.Stock.Notifications.Domain;

internal sealed class StockSoldEventHandler
    : INotificationHandler<DomainEventNotification<StockSoldEvent>>
{
    private readonly IStockProjection _stockProjection;
    private readonly IIntegrationOutboxWriter _outboxWriter;
    private readonly IStockReadRepository _stockReadRepository;
    
    public StockSoldEventHandler(
        IStockProjection stockProjection,
        IIntegrationOutboxWriter outboxWriter,
        IStockReadRepository stockReadRepository)
    {
        _stockProjection     = stockProjection; 
        _outboxWriter        = outboxWriter; 
        _stockReadRepository = stockReadRepository; 
    }
    
    public async Task Handle(
        DomainEventNotification<StockSoldEvent> notification, 
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        
        await _stockProjection.SellAsync(
            now:     e.OccurredAt,
            stockId: e.StockId,
            count:   e.Count,
            cancellationToken: cancellationToken);
        
        var result = await _stockReadRepository.GetAsync(
            stockId: e.StockId,
            cancellationToken: cancellationToken);

        if (result is null)
        {
            throw new NotFoundException(
                name: typeof(VariantStockReadModel), e.StockId);
        }

        _outboxWriter.Stage(
            new StockAvailabilityChangedIntegrationEvent(
                OccurredAt: e.OccurredAt,
                VariantId:  result.VariantId,
                SizeId:     result.SizeId,
                Count:      result.InStock,
                Sales:      result.Sales));
    }
}