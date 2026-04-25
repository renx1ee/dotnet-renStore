namespace RenStore.Inventory.Application.Features.Reservation.Queries.FindExpired;

// FindExpired
public sealed record FindExpiredReservationsQuery()
    : IRequest<IReadOnlyList<VariantReservationDto>>;