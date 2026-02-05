using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository with working with <see cref="ProductDetail"/>.
/// Provide basic CRUD operations and data retrieval methods with sorting and pagination.
/// </summary>
public interface IProductDetailRepository
{
    /// <summary>
    /// Create a new product detail in the database.
    /// </summary>
    /// <param name="detail">The product detail for create.</param>
    /// <param name="cancellationToken">CancellationToken.</param>
    /// <returns>ID of created entity.</returns>
    Task<Guid> CreateAsync(
        ProductDetail detail,
        CancellationToken cancellationToken);
    /// <summary>
    /// Edit an existing product detail in the database.
    /// </summary>
    /// <param name="detail">The product detail with updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when product detail is not found.</exception>
    Task UpdateAsync(
        ProductDetail detail,
        CancellationToken cancellationToken);
    /// <summary>
    /// Delete a product detail from database by ID.
    /// </summary>
    /// <param name="id">The unique identifier of product detail.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when product detail is not found.</exception>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all product details with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="ProductDetailSortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true. Defaults to false.</param>
    /// <returns>A collection of matching the product detail entities.</returns>
    Task<IEnumerable<ProductDetail>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductDetailSortBy sortBy = ProductDetailSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Finds a product detail by ID.
    /// </summary>
    /// <param name="id">The product detail unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The product detail entity if found; overwise <c>null</c>.</returns>
    Task<ProductDetail?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a product detail by ID.
    /// </summary>
    /// <param name="id">The product detail unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The product detail entity if found.</returns>
    /// /// <exception cref="NotFoundException">Thrown when product detail is not found.</exception>
    Task<ProductDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}