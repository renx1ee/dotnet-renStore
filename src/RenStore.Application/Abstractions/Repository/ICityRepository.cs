using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository for working with <see cref="City"/>.
/// Provide basic CRUD operations and data retrieval methods with sorting and pagination.
/// </summary>
public interface ICityRepository
{
    /// <summary>
    /// Create a new city in the database.
    /// </summary>
    /// <param name="city">The city entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>ID of the created entity.</returns>
    Task<int> CreateAsync(City city, CancellationToken cancellationToken);
    /// <summary>
    /// Update an existing city in the database.
    /// </summary>
    /// <param name="city">The city entity with the updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when city is not found.</exception>
    Task UpdateAsync(City city, CancellationToken cancellationToken);
    /// <summary>
    /// Delete a city by ID.
    /// </summary>
    /// <param name="id">The unique identifier of city.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when city is not found.</exception>
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all cities with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="CitySortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of matching the city entities.</returns>
    Task<IEnumerable<City?>> FindAllAsync(
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Finds a city by ID. 
    /// </summary>
    /// <param name="id">The city identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The city entity if found; overwise, <c>null</c>.</returns>
    Task<City?> FindByIdAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a city by ID.
    /// </summary>
    /// <param name="id">The city identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The city entity.</returns>
    /// <exception cref="NotFoundException">Thrown when city is not found.</exception>  
    Task<City?> GetByIdAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Searching categories by name with sorting and pagination.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="CitySortBy.Id"/></param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of matching city entities.</returns>
    Task<IEnumerable<City?>> FindByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Searching cities by name with sorting and pagination.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="CitySortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort by descending order if true.</param>
    /// <returns>A collection of matching city entity.</returns>
    /// <exception cref="NotFoundException">Thrown when city is not found.</exception>
    Task<IEnumerable<City?>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CitySortBy sortBy = CitySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}