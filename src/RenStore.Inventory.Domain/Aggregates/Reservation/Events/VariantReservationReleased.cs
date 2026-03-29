using RenStore.Inventory.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Domain.Aggregates.Reservation.Events;

public sealed record VariantReservationReleased(
    Guid EventId,
    DateTimeOffset OccurredAt,
    ReservationStatus Status)
    : IDomainEvent;