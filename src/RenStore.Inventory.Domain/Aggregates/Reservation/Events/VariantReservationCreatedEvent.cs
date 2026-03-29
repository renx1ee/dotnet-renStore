using RenStore.Inventory.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Domain.Aggregates.Reservation.Events;

public sealed record VariantReservationCreatedEvent(
    Guid EventId,
    Guid ReservationId,
    int Quantity,
    ReservationStatus Status,
    DateTimeOffset OccurredAt,
    DateTimeOffset ExpiresAt,
    Guid VariantId,
    Guid SizeId,
    Guid OrderId)
    : IDomainEvent;