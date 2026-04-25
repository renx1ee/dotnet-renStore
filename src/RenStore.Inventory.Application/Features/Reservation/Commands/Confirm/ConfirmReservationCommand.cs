using RenStore.Inventory.Domain.Enums;

namespace RenStore.Inventory.Application.Features.Reservation.Commands.Confirm;

public sealed record ConfirmReservationCommand(
    Guid ReservationId)
    : IRequest;