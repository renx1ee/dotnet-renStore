using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Delivery.Application.Abstractions;

public interface IEventStore
{
    Task<IReadOnlyList<IDomainEvent>> LoadAsync(
        Guid aggregateId,
        CancellationToken cancellationToken = default);

    Task AppendAsync(
        Guid aggregateId,
        int expectedVersion,
        IReadOnlyList<IDomainEvent> events,
        CancellationToken cancellationToken = default);
}