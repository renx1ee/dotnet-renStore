using RenStore.Inventory.Domain.Enums;

namespace RenStore.Inventory.Application.Features.Reservation.Commands.Cancel;

public sealed record CancelReservationCommand(
    Guid ReservationId,
    ReservationCancelReason Reason)
    : IRequest;