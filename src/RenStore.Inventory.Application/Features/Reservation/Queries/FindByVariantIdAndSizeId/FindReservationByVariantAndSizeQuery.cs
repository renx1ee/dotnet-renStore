namespace RenStore.Inventory.Application.Features.Reservation.Queries.FindByVariantIdAndSizeId;

public sealed record FindReservationByVariantAndSizeQuery(
    Guid VariantId,
    Guid SizeId)
    : IRequest<VariantReservationDto?>;