using RenStore.Identity.Application.Abstractions;
using RenStore.Identity.Domain.Interfaces;

namespace RenStore.Identity.Persistence.Write.Repositories;

internal sealed class ApplicationUserRepository(
    IEventStore eventStore)
    : IApplicationUserRepository
{
    public async Task<ApplicationUser?> GetAsync(
        Guid              userId,
        CancellationToken cancellationToken)
    {
        if (userId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(userId));

        var events = await eventStore.LoadAsync(userId, cancellationToken);

        return events.Count == 0
            ? null
            : ApplicationUser.Rehydrate(events);
    }

    public async Task SaveAsync(
        ApplicationUser   user,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);

        var uncommittedEvents = user.GetUncommittedEvents();

        if (uncommittedEvents.Count == 0) return;

        await eventStore.AppendAsync(
            aggregateId:     user.Id,
            expectedVersion: user.Version,
            events:          uncommittedEvents.ToList(),
            cancellationToken);

        user.UncommittedEventsClear();
    }
}