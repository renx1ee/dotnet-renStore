using MediatR;
using RenStore.Inventory.Application.Abstractions;
using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Stock;

namespace RenStore.Inventory.Persistence.Write.Repositories;

internal sealed class StockRepository 
    : RenStore.Inventory.Domain.Interfaces.Repository.IStockRepository
{
    private readonly IEventStore _eventStore;
    private readonly IMediator _mediator;
    
    public StockRepository(
        IEventStore eventStore,
        IMediator mediator)
    {
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _mediator = mediator     ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task<VariantStock?> GetAsync(
        Guid stockId,
        CancellationToken cancellationToken)
    {
        if (stockId == Guid.Empty)
            throw new InvalidOperationException(nameof(stockId));
        
        var events = await _eventStore.LoadAsync(
            aggregateId: stockId,
            cancellationToken: cancellationToken);

        if (events.Count == 0) return null;

        return VariantStock.Rehydrate(events);
    }

    public async Task SaveAsync(
        VariantStock stock,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stock);

        var uncommittedEvents = stock.GetUncommittedEvents();

        if (uncommittedEvents.Count == 0) return;

        await _eventStore.AppendAsync(
            aggregateId: stock.Id,
            expectedVersion: stock.Version,
            events: uncommittedEvents.ToList(),
            cancellationToken: cancellationToken);

        foreach (var domainEvent in uncommittedEvents)
        {
            var notificationType = typeof(DomainEventNotification<>)
                .MakeGenericType(domainEvent.GetType());

            var notification = (INotification)Activator
                .CreateInstance(notificationType, domainEvent)!;

            await _mediator.Publish(notification, cancellationToken);
        }
    }
}