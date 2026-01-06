using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Interfaces;

public interface ISortingCenterQuery
{
    Task<IReadOnlyList<SortingCenterReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        SortingCenterSortBy sortBy = SortingCenterSortBy.Id,
        bool descending = false,
        bool? isDeleted = null);

    Task<SortingCenterReadModel?> FindByIdAsync(
        long id,
        CancellationToken cancellationToken);

    Task<SortingCenterReadModel> GetByIdAsync(
        long id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<SortingCenterReadModel>> FindByAddressIdAsync(
        Guid addressId,
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        SortingCenterSortBy sortBy = SortingCenterSortBy.Id,
        bool descending = false,
        bool? isDeleted = null);

    Task<IReadOnlyList<SortingCenterReadModel>> GetByAddressIdAsync(
        Guid addressId,
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        SortingCenterSortBy sortBy = SortingCenterSortBy.Id,
        bool descending = false,
        bool? isDeleted = null);
}