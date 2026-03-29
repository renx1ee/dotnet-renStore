using RenStore.Inventory.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Domain.Aggregates.Reservation.Events;

public sealed record VariantReservationExpiredEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    ReservationStatus Status)
    : IDomainEvent;