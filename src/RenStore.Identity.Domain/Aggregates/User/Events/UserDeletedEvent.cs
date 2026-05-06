using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Domain.Aggregates.User.Events;

public sealed record UserDeletedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           UserId) : IDomainEvent;