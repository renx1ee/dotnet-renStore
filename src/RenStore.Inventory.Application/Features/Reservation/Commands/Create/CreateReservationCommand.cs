namespace RenStore.Inventory.Application.Features.Reservation.Commands.Create;

public sealed record CreateReservationCommand(
    int Quantity,
    Guid VariantId,
    Guid SizeId,
    Guid OrderId)
    : IRequest<Guid>;