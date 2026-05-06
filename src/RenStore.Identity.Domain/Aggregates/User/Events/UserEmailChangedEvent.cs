using RenStore.Identity.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Domain.Aggregates.User.Events;

public sealed record UserEmailChangedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           UserId,
    UserEmail      Email) : IDomainEvent;