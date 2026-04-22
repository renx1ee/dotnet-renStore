using RenStore.Inventory.Application.ReadModels;
using RenStore.Inventory.Domain.Enums.Sorting;

namespace RenStore.Inventory.Application.Abstractions.Queries;

public interface IReservationQuery
{
    Task<VariantReservationDto?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<VariantReservationDto>> FindByVariantIdAsync(
        Guid variantId,
        ReservationSortBy sortBy = ReservationSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? includeExpired = null,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default);

    Task<VariantReservationDto?> FindByVariantAndSizeAsync(
        Guid variantId,
        Guid sizeId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<VariantReservationDto>> FindByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken);
    
    Task<IReadOnlyList<VariantReservationDto>> FindExpiredReservationsAsync(
        CancellationToken cancellationToken);
}