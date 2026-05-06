using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Domain.Aggregates.User.Events;

public sealed record UserPasswordChangedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           UserId,
    string         PasswordHash) : IDomainEvent;