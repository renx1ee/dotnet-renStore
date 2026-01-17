using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository for working with <see cref="SubCategory"/>.
/// Provide basic CRUD operations and data retrieval methods with sorting and pagination.
/// </summary>
public interface ISubCategoryRepository
{
    /// <summary>
    /// Create a new subcategory in the database.
    /// </summary>
    /// <param name="subCategory">The subcategory entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>ID of the created entity.</returns>
    Task<int> CreateAsync(SubCategory subCategory, CancellationToken cancellationToken);
    /// <summary>
    /// Edit an existing subcategory in the database.
    /// </summary>
    /// <param name="subCategory">The subcategory entity with the updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when subcategory is not found.</exception>
    Task UpdateAsync(SubCategory subCategory, CancellationToken cancellationToken);
    /// <summary>
    /// Delete a subcategory by ID.
    /// </summary>
    /// <param name="id">The unique identifier of subcategory.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when subcategory is not found.</exception>
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all subcategories with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="SubCategorySortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of matching the subcategory entities.</returns>
    Task<IEnumerable<SubCategory>> FindAllAsync(
        CancellationToken cancellationToken,
        SubCategorySortBy sortBy = SubCategorySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Finds a subcategory by ID. 
    /// </summary>
    /// <param name="id">The subcategory identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The subcategory entity if found; overwise, <c>null</c>.</returns>
    Task<SubCategory?> FindByIdAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a subcategory by ID.
    /// </summary>
    /// <param name="id">The subcategory identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The subcategory entity.</returns>
    /// <exception cref="NotFoundException">Thrown when subcategory is not found.</exception>
    Task<SubCategory> GetByIdAsync(int id, CancellationToken cancellationToken);
    /// <summary>
    /// Searching subcategories by name with sorting and pagination.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="SubCategorySortBy.Id"/></param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of matching subcategory entities.</returns>
    Task<IEnumerable<SubCategory?>> FindByNameAsync(
        string name,
        CancellationToken cancellationToken,
        SubCategorySortBy sortBy = SubCategorySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Searching subcategories by name with sorting and pagination.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="SubCategorySortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort by descending order if true.</param>
    /// <returns>A collection of matching subcategory entity.</returns>
    /// <exception cref="NotFoundException">Thrown when subcategory is not found.</exception>
    Task<IEnumerable<SubCategory?>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken,
        SubCategorySortBy sortBy = SubCategorySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Searching subcategories by category ID with sorting and pagination.
    /// </summary>
    /// <param name="categoryId">The category ID to search for.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="SubCategorySortBy.Id"/></param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of matching subcategory entities.</returns>
    Task<IEnumerable<SubCategory?>> FindByCategoryIdAsync(
        int categoryId,
        CancellationToken cancellationToken,
        SubCategorySortBy sortBy = SubCategorySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Searching subcategories by category ID with sorting and pagination.
    /// </summary>
    /// <param name="categoryId">The category ID to search for.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="SubCategorySortBy.Id"/></param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of matching subcategory entities.</returns>
    /// <exception cref="NotFoundException">Thrown when subcategory is not found.</exception>
    Task<IEnumerable<SubCategory?>> GetByCategoryIdAsync(
        int categoryId,
        CancellationToken cancellationToken,
        SubCategorySortBy sortBy = SubCategorySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
}