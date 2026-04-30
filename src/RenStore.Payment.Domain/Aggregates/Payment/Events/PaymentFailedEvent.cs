using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Payment.Domain.Aggregates.Payment.Events;

public sealed record PaymentFailedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           PaymentId,
    Guid           AttemptId,
    string         FailureReason,
    string?        ProviderErrorCode) 
    : IDomainEvent;