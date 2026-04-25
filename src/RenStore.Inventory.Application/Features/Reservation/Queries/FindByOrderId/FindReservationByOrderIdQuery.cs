namespace RenStore.Inventory.Application.Features.Reservation.Queries.FindByOrderId;

// FindByOrderId
public sealed record FindReservationByOrderIdQuery(Guid OrderId)
    : IRequest<IReadOnlyList<VariantReservationDto>>;