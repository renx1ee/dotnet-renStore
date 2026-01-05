using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Interfaces;

/// <summary>
/// Query for working with <see cref="RenStore.Delivery.Domain.ReadModels.CityReadModel"/>
/// Provide basic read method with sorting and pagination.
/// </summary>
public interface ICityQuery
{
    /// <summary>
    /// Retrieves all cities with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Default value is <see cref="CitySortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max value is 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted cities. Default is <see cref="null"/>.</param>
    /// <returns>A collection of matching <see cref="CitySortBy"/>.</returns>
    Task<IReadOnlyList<CityReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null);

    /// <summary>
    /// Finds a <see cref="CityReadModel"/> by city ID.
    /// </summary>
    /// <param name="id">City unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Return a <see cref="CityReadModel"/> if found.</returns>
    Task<CityReadModel?> FindByIdAsync(
        int id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Gets a <see cref="CityReadModel"/> by city ID.
    /// </summary>
    /// <param name="id">City unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Return a <see cref="CityReadModel"/> if found.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If city not found.</exception>
    Task<CityReadModel> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves all cities by city name with sorting and pagination.
    /// </summary>
    /// <param name="name">City name to search.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Default value is <see cref="CitySortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max value is 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted cities. Default is <see cref="null"/>.</param>
    /// <returns>A collection of matching <see cref="CitySortBy"/>.</returns>
    Task<IReadOnlyList<CityReadModel>> FindByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null);

    /// <summary>
    /// Gets all cities by city name with sorting and pagination.
    /// </summary>
    /// <param name="name">City name to search.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Default value is <see cref="CitySortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max value is 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted cities. Default is <see cref="null"/>.</param>
    /// <returns>A collection of matching <see cref="CitySortBy"/>.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If city not found.</exception>
    Task<IReadOnlyList<CityReadModel>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null);
    
    /// <summary>
    /// Retrieves all cities by country ID with sorting and pagination. 
    /// </summary>
    /// <param name="countryId">Country ID to search.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Default value is <see cref="CitySortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max value is 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted cities. Default is <see cref="null"/>.</param>
    /// <returns>A collection of matching <see cref="CitySortBy"/>.</returns>
    /// <returns></returns>
    Task<IReadOnlyList<CityReadModel>> FindByCountryIdAsync(
        int countryId,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null);
    
    /// <summary>
    /// Gets all cities by country ID with sorting and pagination.
    /// </summary>
    /// <param name="countryId">Country ID to search.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Default value is <see cref="CitySortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max value is 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted cities. Default is <see cref="null"/>.</param>
    /// <returns>A collection of matching <see cref="CitySortBy"/>.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If city not found.</exception>
    Task<IReadOnlyList<CityReadModel>> GetByCountryIdAsync(
        int countryId,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageSize = 25,
        uint page = 1,
        bool descending = false,
        bool? isDeleted = null);
}