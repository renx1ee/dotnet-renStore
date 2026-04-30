using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Payment.Domain.Aggregates.Payment.Events;

public sealed record PaymentCapturedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           PaymentId,
    string         ExternalPaymentId) 
    : IDomainEvent;