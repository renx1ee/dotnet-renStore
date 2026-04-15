using MediatR;
using RenStore.Order.Application.Abstractions;
using RenStore.Order.Application.Common;

namespace RenStore.Order.Persistence.Write.Repository;

internal sealed class OrderRepository
    : RenStore.Order.Domain.Interfaces.IOrderRepository
{
    private readonly IEventStore _eventStore;
    private readonly IMediator _mediator;
    
    public OrderRepository(
        IEventStore eventStore,
        IMediator mediator)
    {
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _mediator = mediator     ?? throw new ArgumentNullException(nameof(mediator));
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