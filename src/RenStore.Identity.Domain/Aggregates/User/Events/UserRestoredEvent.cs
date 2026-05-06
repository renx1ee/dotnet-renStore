using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Domain.Aggregates.User.Events;

public sealed record UserRestoredEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           UserId) : IDomainEvent;