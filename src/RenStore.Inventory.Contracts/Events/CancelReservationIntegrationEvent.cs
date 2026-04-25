using RenStore.Inventory.Domain.Enums;

namespace RenStore.Inventory.Contracts.Events;

public sealed record CancelReservationIntegrationEvent(
    Guid ReservationId,
    ReservationCancelReason Reason);