using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Interfaces;

/// <summary>
/// Query for working with <see cref="SortingCenterReadModel"/>.
/// Provide basic read method with sorting and pagination.
/// </summary>
public interface ISortingCenterQuery
{
    /// <summary>
    /// Retrieves all sorting centers with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Default value is <see cref="SortingCenterSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max value is 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted sorting center. Default is <see cref="null"/>.</param>
    /// <returns>A collection of matching <see cref="SortingCenterReadModel"/>.</returns>
    Task<IReadOnlyList<SortingCenterReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        SortingCenterSortBy sortBy = SortingCenterSortBy.Id,
        bool descending = false,
        bool? isDeleted = null);

    /// <summary>
    /// Get a <see cref="SortingCenterReadModel"/> by sorting center ID.
    /// </summary>
    /// <param name="id">Sorting center unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Return a <see cref="SortingCenterReadModel"/> if found.</returns>
    Task<SortingCenterReadModel?> FindByIdAsync(
        long id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get a <see cref="SortingCenterReadModel"/> by sorting center ID.
    /// </summary>
    /// <param name="id">Sorting center unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Return a <see cref="SortingCenterReadModel"/> if found.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If sorting center not found.</exception>
    Task<SortingCenterReadModel> GetByIdAsync(
        long id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Finds a <see cref="SortingCenterReadModel"/> by user ID.
    /// </summary>
    /// <param name="addressId">Address identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default is <see cref="SortingCenterSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default is 1.</param>
    /// <param name="pageSize">Number of items per page. Default is 25, Max 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default is false.</param>
    /// <param name="isDeleted">Include deleted sorting center. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="SortingCenterReadModel"/>.</returns>
    Task<IReadOnlyList<SortingCenterReadModel>> FindByAddressIdAsync(
        Guid addressId,
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        SortingCenterSortBy sortBy = SortingCenterSortBy.Id,
        bool descending = false,
        bool? isDeleted = null);

    /// <summary>
    /// Gets a <see cref="SortingCenterReadModel"/> by user ID.
    /// </summary>
    /// <param name="addressId">Address identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default is <see cref="SortingCenterSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default is 1.</param>
    /// <param name="pageSize">Number of items per page. Default is 25, Max 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default is false.</param>
    /// <param name="isDeleted">Include deleted sorting centers. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="SortingCenterReadModel"/>.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If sorting center not found.</exception>
    Task<IReadOnlyList<SortingCenterReadModel>> GetByAddressIdAsync(
        Guid addressId,
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        SortingCenterSortBy sortBy = SortingCenterSortBy.Id,
        bool descending = false,
        bool? isDeleted = null);
}