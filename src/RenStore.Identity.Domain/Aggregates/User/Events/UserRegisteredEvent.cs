using RenStore.Identity.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Domain.Aggregates.User.Events;

public sealed record UserRegisteredEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           UserId,
    UserName       Name,
    UserEmail      Email,
    string         PasswordHash) 
    : IDomainEvent;