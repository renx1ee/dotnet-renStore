using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Interfaces;
/// <summary>
/// Query for working with <see cref="PickupPointReadModel"/>.
/// Provide basic read method with sorting and pagination.
/// </summary>
public interface IPickupPointQuery
{
    /// <summary>
    /// Retrieves all pickup points with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Default value is <see cref="PickupSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max value is 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted pickup points. Default is <see cref="null"/>.</param>
    /// <returns>A collection of matching <see cref="PickupPointReadModel"/>.</returns>
    Task<IReadOnlyList<PickupPointReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        PickupSortBy sortBy = PickupSortBy.Id,
        bool descending = false,
        bool? isDeleted = null);

    /// <summary>
    /// Get a <see cref="PickupPointReadModel"/> by pickup points ID.
    /// </summary>
    /// <param name="id">Pickup point unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Return a <see cref="PickupPointReadModel"/> if found.</returns>
    Task<PickupPointReadModel?> FindByIdAsync(
        long id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get a <see cref="PickupPointReadModel"/> by pickup points ID.
    /// </summary>
    /// <param name="id">Pickup point unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Return a <see cref="PickupPointReadModel"/> if found.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If pickup point not found.</exception>
    Task<PickupPointReadModel> GetByIdAsync(
        long id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Finds a <see cref="PickupPointReadModel"/> by user ID.
    /// </summary>
    /// <param name="addressId">Address identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default is <see cref="PickupSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default is 1.</param>
    /// <param name="pageSize">Number of items per page. Default is 25, Max 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default is false.</param>
    /// <param name="isDeleted">Include deleted pickup points. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="PickupPointReadModel"/>.</returns>
    Task<IReadOnlyList<PickupPointReadModel>> FindByAddressIdAsync(
        Guid addressId,
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        PickupSortBy sortBy = PickupSortBy.Id,
        bool descending = false,
        bool? isDeleted = null);

    /// <summary>
    /// Gets a <see cref="PickupPointReadModel"/> by user ID.
    /// </summary>
    /// <param name="addressId">Address identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default is <see cref="PickupSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default is 1.</param>
    /// <param name="pageSize">Number of items per page. Default is 25, Max 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default is false.</param>
    /// <param name="isDeleted">Include deleted pickup points. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="PickupPointReadModel"/>.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If pickup point not found.</exception>
    Task<IReadOnlyList<PickupPointReadModel>> GetByAddressIdAsync(
        Guid addressId,
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        PickupSortBy sortBy = PickupSortBy.Id,
        bool descending = false,
        bool? isDeleted = null);
}