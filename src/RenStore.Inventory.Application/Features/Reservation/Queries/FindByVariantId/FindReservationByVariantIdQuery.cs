using RenStore.Inventory.Domain.Enums.Sorting;

namespace RenStore.Inventory.Application.Features.Reservation.Queries.FindByVariantId;

public sealed record FindReservationByVariantIdQuery(
    Guid VariantId,
    ReservationSortBy SortBy = ReservationSortBy.Id,
    uint Page = 1,
    uint PageSize = 25,
    bool Descending = false,
    bool? IncludeExpired = null,
    bool? IsDeleted = null)
    : IRequest<IReadOnlyList<VariantReservationDto>>;