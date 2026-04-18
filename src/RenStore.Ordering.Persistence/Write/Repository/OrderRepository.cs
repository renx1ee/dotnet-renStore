using MediatR;
using RenStore.Order.Application.Abstractions;
using RenStore.Order.Application.Common;

namespace RenStore.Order.Persistence.Write.Repository;

internal sealed class OrderRepository
    : RenStore.Order.Domain.Interfaces.IOrderRepository
{
    private readonly IEventStore _eventStore;
    
    public OrderRepository(IEventStore eventStore)
    {
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }

    public async Task<Domain.Aggregates.Order.Order?> GetAsync(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        if (orderId == Guid.Empty)
            throw new InvalidOperationException(nameof(orderId));
        
        var events = await _eventStore.LoadAsync(
            aggregateId: orderId,
            cancellationToken: cancellationToken);

        if (events.Count == 0) return null;

        return Domain.Aggregates.Order.Order.Rehydrate(events);
    }

    public async Task SaveAsync(
        Domain.Aggregates.Order.Order order,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(order);

        var uncommittedEvents = order.GetUncommittedEvents();

        if (uncommittedEvents.Count == 0) return;

        await _eventStore.AppendAsync(
            aggregateId: order.Id,
            expectedVersion: order.Version,
            events: uncommittedEvents.ToList(),
            cancellationToken: cancellationToken);
    }
}