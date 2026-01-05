using RenStore.Delivery.Application.Features.Address.Queries;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Interfaces;

/// <summary>
/// Query for working with <see cref="AddressReadModel"/>.
/// Provide basic read method with sorting and pagination.
/// </summary>
public interface IAddressQuery
{
    /// <summary>
    /// Retrieves all addresses with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Default value is <see cref="AddressSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max value is 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted addresses. Default is <see cref="null"/>.</param>
    /// <returns>A collection of matching <see cref="AddressReadModel"/>.</returns>
    Task<IReadOnlyList<AddressReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);
    
    /// <summary>
    /// Searching all addresses by criteria with sorting and pagination.
    /// </summary>
    /// <param name="criteria">Criteria to search with.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default value is <see cref="AddressSortBy.Id"/>.</param>
    /// <param name="pageSize">Number of items per page. Default is 25, Max is 1000.</param>
    /// <param name="page">Page number (1-based). Default is 1.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted addresses. Default is <see cref="null"/>.</param>
    /// <returns>A collection of items matching <see cref="AddressReadModel"/>.</returns>
    Task<IReadOnlyList<AddressReadModel>> SearchAsync(
        AddressSearchCriteria criteria,
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null);
    
    /// <summary>
    /// Finds a <see cref="AddressReadModel"/> by address ID.
    /// </summary>
    /// <param name="id">Address unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Return a <see cref="AddressReadModel"/> if found.</returns>
    Task<AddressReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Get a <see cref="AddressReadModel"/> by address ID.
    /// </summary>
    /// <param name="id">Address unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Return a <see cref="AddressReadModel"/> if found.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If address not found.</exception>
    Task<AddressReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Finds a <see cref="AddressReadModel"/> by user ID.
    /// </summary>
    /// <param name="userId">User unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default is <see cref="AddressSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default is 1.</param>
    /// <param name="pageSize">Number of items per page. Default is 25, Max 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default is false.</param>
    /// <param name="isDeleted">Include deleted addresses. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="AddressReadModel"/>.</returns>
    Task<IReadOnlyList<AddressReadModel>> FindByUserIdAsync(
        string userId,
        CancellationToken cancellationToken,
        AddressSortBy sortBy = AddressSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);
    
    /// <summary>
    /// Checking if an address exists.
    /// </summary>
    /// <param name="id">Address unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>True if address is found.</returns>
    Task<bool> IsExists(
        Guid id,
        CancellationToken cancellationToken);
}