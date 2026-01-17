using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository with working with <see cref="ProductClothSizeEntity"/>.
/// Provide basic CRUD operations and data retrieval methods with sorting and pagination.
/// </summary>
public interface IProductClothSizeRepository
{
    /// <summary>
    /// Create a new product cloth sizes in the database.
    /// </summary>
    /// <param name="clothSize">The product cloth size for create.</param>
    /// <param name="cancellationToken">CancellationToken.</param>
    /// <returns>ID of created entity.</returns>
    Task<Guid> CreateAsync(
        ProductClothSizeEntity clothSize,
        CancellationToken cancellationToken);
    /// <summary>
    /// Edit an existing product cloth size in the database.
    /// </summary>
    /// <param name="clothSize">The product cloth size with updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when product cloth size is not found.</exception>
    Task UpdateAsync(
        ProductClothSizeEntity clothSize,
        CancellationToken cancellationToken);
    /// <summary>
    /// Delete a product cloth size from database by ID.
    /// </summary>
    /// <param name="id">The unique identifier of product cloth size.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when product cloth size is not found.</exception>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all product cloth size with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="ProductClothSizeSortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true. Defaults to false.</param>
    /// <returns>A collection of matching the product cloth size entities.</returns>
    Task<IEnumerable<ProductClothSizeEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductClothSizeSortBy sortBy = ProductClothSizeSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Finds a product cloth size by ID.
    /// </summary>
    /// <param name="id">The product cloth size unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The product cloth size entity if found; overwise <c>null</c>.</returns>
    Task<ProductClothSizeEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a product cloth size by ID.
    /// </summary>
    /// <param name="id">The product cloth size unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The product cloth size entity if found.</returns>
    /// <exception cref="NotFoundException">Thrown when product cloth size is not found.</exception>
    Task<ProductClothSizeEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}