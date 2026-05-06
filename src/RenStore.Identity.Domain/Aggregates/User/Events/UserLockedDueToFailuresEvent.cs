using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Domain.Aggregates.User.Events;

public sealed record UserLockedDueToFailuresEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           UserId,
    DateTimeOffset LockoutEnd) : IDomainEvent;