namespace RenStore.Inventory.Application.Features.Reservation.Commands.Expire;

public sealed record ExpireReservationCommand(
    Guid ReservationId)
    : IRequest;