using RenStore.Inventory.Domain.Enums.Sorting;

namespace RenStore.Inventory.WebApi.Requests.Reservation;

public sealed record FindReservationByVariantIdRequest(
    ReservationSortBy SortBy = ReservationSortBy.Id,
    uint Page = 1,
    uint PageSize = 25,
    bool Descending = false,
    bool? IncludeExpired = null,
    bool? IsDeleted = null);