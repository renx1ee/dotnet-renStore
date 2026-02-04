using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository with working with <see cref="ProductCloth"/>.
/// Provide basic CRUD operations and data retrieval methods with sorting and pagination.
/// </summary>
public interface IProductClothRepository
{
    /// <summary>
    /// Create a new product cloth in the database.
    /// </summary>
    /// <param name="cloth">The product cloth for create.</param>
    /// <param name="cancellationToken">CancellationToken.</param>
    /// <returns>ID of created entity.</returns>
    Task<Guid> CreateAsync(
        ProductCloth cloth,
        CancellationToken cancellationToken);
    /// <summary>
    /// Edit an existing product cloth in the database.
    /// </summary>
    /// <param name="cloth">The product cloth with updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when product cloth is not found.</exception>
    Task UpdateAsync(
        ProductCloth cloth,
        CancellationToken cancellationToken);
    /// <summary>
    /// Delete a product cloth from database by ID.
    /// </summary>
    /// <param name="id">The unique identifier of product cloth.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when product cloth is not found.</exception>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all product clothes with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="ProductClothSortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true. Defaults to false.</param>
    /// <returns>A collection of matching the product cloth entities.</returns>
    Task<IEnumerable<ProductCloth>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductClothSortBy sortBy = ProductClothSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Finds a product cloth by ID.
    /// </summary>
    /// <param name="id">The product cloth unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The product cloth entity if found; overwise <c>null</c>.</returns>
    Task<ProductCloth?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a product cloth by ID.
    /// </summary>
    /// <param name="id">The product cloth unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The product cloth entity if found.</returns>
    /// <exception cref="NotFoundException">Thrown when product cloth is not found.</exception>
    Task<ProductCloth?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}