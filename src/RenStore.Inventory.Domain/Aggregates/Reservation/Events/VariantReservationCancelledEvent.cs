using RenStore.Inventory.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Domain.Aggregates.Reservation.Events;

public sealed record VariantReservationCancelledEvent(
    Guid EventId,
    Guid Id,
    DateTimeOffset OccurredAt,
    ReservationStatus Status,
    ReservationCancelReason CancelReason)
    : IDomainEvent;