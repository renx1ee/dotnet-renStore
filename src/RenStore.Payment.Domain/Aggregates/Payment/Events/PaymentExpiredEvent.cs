using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Payment.Domain.Aggregates.Payment.Events;

public sealed record PaymentExpiredEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           PaymentId) 
    : IDomainEvent;