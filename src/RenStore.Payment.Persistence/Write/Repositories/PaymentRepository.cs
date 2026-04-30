using RenStore.Payment.Application.Abstractions;
using RenStore.Payment.Domain.Interfaces;

namespace RenStore.Payment.Persistence.Write.Repositories;

internal sealed class PaymentRepository : IPaymentRepository
{
    private readonly IEventStore _eventStore;

    public PaymentRepository(IEventStore eventStore)
    {
        _eventStore = eventStore
                      ?? throw new ArgumentNullException(nameof(eventStore));
    }

    public async Task<Domain.Aggregates.Payment.Payment?> GetAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));

        var events = await _eventStore.LoadAsync(id, cancellationToken);

        if (events.Count == 0) return null;

        return Domain.Aggregates.Payment.Payment.Rehydrate(events);
    }

    public async Task SaveAsync(
        Domain.Aggregates.Payment.Payment payment,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(payment);

        var uncommittedEvents = payment.GetUncommittedEvents();

        if (uncommittedEvents.Count == 0)
            return;

        await _eventStore.AppendAsync(
            aggregateId:     payment.Id,
            expectedVersion: payment.Version,
            events:          uncommittedEvents.ToList(),
            cancellationToken);

        payment.UncommittedEventsClear();
    }
}