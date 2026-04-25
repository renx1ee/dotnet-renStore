namespace RenStore.Inventory.Application.Features.Reservation.Queries.FindById;

public sealed record FindReservationByIdQuery(Guid Id)
    : IRequest<VariantReservationDto?>;