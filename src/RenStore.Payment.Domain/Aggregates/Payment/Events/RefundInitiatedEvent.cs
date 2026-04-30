using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Payment.Domain.Aggregates.Payment.Events;

public sealed record RefundInitiatedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           PaymentId,
    Guid           RefundId,
    decimal        Amount,
    Currency       Currency,
    string         Reason) 
    : IDomainEvent;