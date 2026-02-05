using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository for working with <see cref="Category"/>.
/// Provides basic CRUD operations and data retrieval methods with sorting and pagination.
/// </summary>
/// <remarks>
/// Initializes a new repository instance.
/// </remarks>
/// <param name="context">Database context.</param>
/// <param name="connectionString">Database connection string.</param>
public interface ICategoryRepository
{
    /// <summary>
    /// Create a new Category in the database.
    /// </summary>
    /// <param name="category">The category entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>ID of the created entity.</returns>
    Task<int> CreateAsync(Category category, CancellationToken cancellationToken);
    /// <summary>
    /// Edit an existing category in the database.
    /// </summary>
    /// <param name="category">The category entity with updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when category is not found.</exception>
    Task UpdateAsync(Category category, CancellationToken cancellationToken);
    /// <summary>
    /// Delete a category by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the category.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when category is not found.</exception>
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all categories with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="CategorySortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of category entities.</returns>
    Task<IEnumerable<Category>> FindAllAsync(CancellationToken cancellationToken,
        CategorySortBy sortBy = CategorySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Finds a category by ID.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The category entity if found; otherwise, <c>null</c>.</returns>
    Task<Category?> FindByIdAsync( int id, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a category by ID.
    /// </summary>
    /// <param name="id">The category identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The category entity.</returns>
    /// <exception cref="NotFoundException">Thrown when category is not found.</exception>
    Task<Category> GetByIdAsync( int id, CancellationToken cancellationToken);
    /// <summary>
    /// Searches categories by name with sorting and pagination.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="CategorySortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of matching category entities.</returns>
    Task<IEnumerable<Category?>> FindByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CategorySortBy sortBy = CategorySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Searches categories by name with sorting and pagination.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="CategorySortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of matching category entities.</returns>
    /// <exception cref="NotFoundException">Thrown when category is not found.</exception>
    Task<IEnumerable<Category?>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken,
        CategorySortBy sortBy = CategorySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}