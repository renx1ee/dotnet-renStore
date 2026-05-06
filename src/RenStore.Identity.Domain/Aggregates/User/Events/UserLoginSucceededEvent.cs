using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Domain.Aggregates.User.Events;

public sealed record UserLoginSucceededEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           UserId) : IDomainEvent;