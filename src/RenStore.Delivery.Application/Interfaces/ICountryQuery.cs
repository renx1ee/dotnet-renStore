using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Interfaces;

/// <summary>
/// Query for working with <see cref="CountryReadModel"/>.
/// Provide basic read method with sorting and pagination.
/// </summary>
public interface ICountryQuery
{
    /// <summary>
    /// Retrieves all countries with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default value is <see cref="CountrySortBy.Id"/>.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max is 1000.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="descending">Sort in descending order if true. Default value if true.</param>
    /// <param name="isDeleted">Include deleted country. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="Country"/>.</returns>
    Task<IReadOnlyList<CountryReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null);
    
    /// <summary>
    /// Finds a <see cref="Country"/> entity by country ID.
    /// </summary>
    /// <param name="id">Country unique identifier in the database.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns><see cref="Country"/> if found.</returns>
    Task<CountryReadModel?> FindByIdAsync(
        int id,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Gets a <see cref="Country"/> entity by country ID.
    /// </summary>
    /// <param name="id">Country unique identifier in the database.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns><see cref="Country"/> if found.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If country not found.</exception>
    Task<CountryReadModel?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Finds a <see cref="Country"/> entity by country ID.
    /// </summary>
    /// <param name="name">Country name to find.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default value is <see cref="CountrySortBy.Id"/>.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max is 1000.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted country. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="Country"/>.</returns>
    Task<IReadOnlyList<CountryReadModel>> FindByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null);
    
    /// <summary>
    /// Finds a <see cref="Country"/> entity by country ID.
    /// </summary>
    /// <param name="name">Country name to find.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default value is <see cref="CountrySortBy.Id"/>.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max is 1000.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted country. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="Country"/>.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If country not found.</exception>
    Task<IEnumerable<CountryReadModel>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Name,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null);
    
    /// <summary>
    /// Finds <see cref="Country"/> entity by city ID.
    /// </summary>
    /// <param name="cityId">City ID for find.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default value is <see cref="CountrySortBy.Id"/>.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max 1000.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted country. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="Country"/> if found.</returns>
    Task<IReadOnlyList<CountryReadModel>> FindByCityIdAsync(
        int cityId,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null);
    
    /// <summary>
    /// Gets <see cref="Country"/> entity by city ID.
    /// </summary>
    /// <param name="cityId">City ID for find.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default value is <see cref="CountrySortBy.Id"/>.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max 1000.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted country. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="Country"/> if found.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If country not found.</exception>
    Task<IReadOnlyList<CountryReadModel>> GetByCityIdAsync(
        int cityId,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null);
}