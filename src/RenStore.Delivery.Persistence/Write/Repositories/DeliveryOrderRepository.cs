using RenStore.Delivery.Application.Abstractions;
using RenStore.Delivery.Domain.Interfaces;

namespace RenStore.Delivery.Persistence.Write.Repositories;

internal sealed class DeliveryOrderRepository(
    IEventStore            eventStore)
    : IDeliveryOrderRepository
{
    public async Task<Domain.Aggregates.DeliveryOrder.DeliveryOrder?> GetAsync(
        Guid              id,
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));

        var events = await eventStore.LoadAsync(id, cancellationToken);

        if (events.Count == 0) return null;

        return Domain.Aggregates.DeliveryOrder.DeliveryOrder.Rehydrate(events);
    }

    public async Task SaveAsync(
        Domain.Aggregates.DeliveryOrder.DeliveryOrder order,
        CancellationToken                              cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(order);

        var uncommittedEvents = order.GetUncommittedEvents();

        if (uncommittedEvents.Count == 0) return;

        await eventStore.AppendAsync(
            aggregateId:     order.Id,
            expectedVersion: order.Version,
            events:          uncommittedEvents.ToList(),
            cancellationToken);

        order.UncommittedEventsClear();
    }
}