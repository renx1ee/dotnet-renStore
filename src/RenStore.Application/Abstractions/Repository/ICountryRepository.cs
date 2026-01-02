using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository for working with <see cref="Country"/>.
/// Provide basic CRUD operations and data retrieval methods with sorting and paginations.
/// </summary>
public interface ICountryRepository
{
    /// <summary>
    /// Create a new country it the database.
    /// </summary>
    /// <param name="country">The country entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>ID of the created entity.</returns>
    Task<int> CreateAsync(Country country, CancellationToken cancellationToken);
    /// <summary>
    /// Update an existing country in the database.
    /// </summary>
    /// <param name="country">The country entity with the updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when country is not found.</exception>
    Task UpdateAsync(Country country, CancellationToken cancellationToken);
    /// <summary>
    /// Delete a country by ID.
    /// </summary>
    /// <param name="id">The unique identifier of country.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when country is not found.</exception>
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all countries with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="CountrySortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of matching the country entities.</returns>
    Task<IEnumerable<Country?>> FindAllAsync(
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Finds a country by ID. 
    /// </summary>
    /// <param name="id">The country identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The country entity if found; overwise, <c>null</c>.</returns>
    Task<Country?> FindByIdAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a country by ID.
    /// </summary>
    /// <param name="id">The country identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The country entity.</returns>
    /// <exception cref="NotFoundException">Thrown when country is not found.</exception>
    Task<Country?> GetByIdAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Searching countries by name with sorting and pagination.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="CountrySortBy.Id"/></param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of matching country entities.</returns>
    Task<IEnumerable<Country?>> FindByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Name,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Searching countries by name with sorting and pagination.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="CountrySortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort by descending order if true.</param>
    /// <returns>A collection of matching country entity.</returns>
    /// <exception cref="NotFoundException">Thrown when country is not found.</exception>
    Task<IEnumerable<Country?>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CountrySortBy sortBy = CountrySortBy.Name,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}