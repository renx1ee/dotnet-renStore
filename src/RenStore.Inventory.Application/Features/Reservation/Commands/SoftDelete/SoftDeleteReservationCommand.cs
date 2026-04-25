namespace RenStore.Inventory.Application.Features.Reservation.Commands.SoftDelete;

public sealed record SoftDeleteReservationCommand(
    Guid ReservationId)
    : IRequest;