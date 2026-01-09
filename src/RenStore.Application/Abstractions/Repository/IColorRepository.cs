using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository for working with <see cref="ColorEntity"/>.
/// Provides basic CRUD operations and data retrieval methods with sorting and pagination.
/// </summary>
public interface IColorRepository
{
    /// <summary>
    /// Create a new collr in the database.
    /// </summary>
    /// <param name="color">The color entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>ID of the created entity.</returns>
    Task<int> CreateAsync(ColorEntity color, CancellationToken cancellationToken);
    /// <summary>
    /// Edit an existing color in the database.
    /// </summary>
    /// <param name="color">The color entity with updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when color is not found.</exception>
    Task UpdateAsync(ColorEntity color, CancellationToken cancellationToken);
    /// <summary>
    /// Delete a color by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the color.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when color is not found.</exception>
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all colors with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="ColorSortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <param name="isBlocked">Gets only blocked or unblocked or all color.</param>
    /// <returns>A collection of color entities.</returns>
    Task<IEnumerable<ColorEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ColorSortBy sortBy = ColorSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Finds a color by ID.
    /// </summary>
    /// <param name="id">The color identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The color entity if found; otherwise, <c>null</c>.</returns>
    Task<ColorEntity?> FindByIdAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a color by ID.
    /// </summary>
    /// <param name="id">The color identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The color entity.</returns>
    /// <exception cref="NotFoundException">Thrown when color is not found.</exception>
    Task<ColorEntity> GetByIdAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Searches colors by name with sorting and pagination.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="ColorSortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of matching color entities.</returns>
    Task<IEnumerable<ColorEntity?>> FindByNameAsync(
        string name,
        CancellationToken cancellationToken,
        ColorSortBy sortBy = ColorSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Searches colors by name with sorting and pagination.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="ColorSortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of matching color entities.</returns>
    /// <exception cref="NotFoundException">Thrown when color is not found.</exception>
    Task<IEnumerable<ColorEntity?>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken,
        ColorSortBy sortBy = ColorSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}