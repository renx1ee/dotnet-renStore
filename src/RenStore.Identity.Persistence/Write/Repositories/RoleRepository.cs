using RenStore.Identity.Application.Abstractions;
using RenStore.Identity.Domain.Aggregates.Role;
using RenStore.Identity.Domain.Interfaces;

namespace RenStore.Identity.Persistence.Write.Repositories;

internal sealed class RoleRepository(
    IEventStore eventStore)
    : IRoleRepository
{
    public async Task<ApplicationRole?> GetAsync(
        Guid              roleId,
        CancellationToken cancellationToken)
    {
        if (roleId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(roleId));

        var events = await eventStore.LoadAsync(roleId, cancellationToken);

        return events.Count == 0
            ? null
            : ApplicationRole.Rehydrate(events);
    }

    public async Task SaveAsync(
        ApplicationRole   applicationRole,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(applicationRole);

        var uncommittedEvents = applicationRole.GetUncommittedEvents();

        if (uncommittedEvents.Count == 0) return;

        await eventStore.AppendAsync(
            aggregateId:     applicationRole.Id,
            expectedVersion: applicationRole.Version,
            events:          uncommittedEvents.ToList(),
            cancellationToken);

        applicationRole.UncommittedEventsClear();
    }
}