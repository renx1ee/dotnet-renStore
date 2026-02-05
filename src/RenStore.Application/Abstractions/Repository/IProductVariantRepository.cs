using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository with working with <see cref="ProductVariant"/>.
/// Provide basic CRUD operations and data retrieval methods with sorting and pagination.
/// </summary>
public interface IProductVariantRepository
{
    /// <summary>
    /// Create a new product variatn in the database.
    /// </summary>
    /// <param name="productVariant">The product variant for create.</param>
    /// <param name="cancellationToken">CancellationToken.</param>
    /// <returns>ID of created entity.</returns>
    Task<Guid> CreateAsync(
        ProductVariant productVariant,
        CancellationToken cancellationToken);
    /// <summary>
    /// Edit an existing product variant in the database.
    /// </summary>
    /// <param name="productVariant">The product variant with updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when product variant is not found.</exception>
    Task UpdateAsync(
        ProductVariant productVariant,
        CancellationToken cancellationToken);
    /// <summary>
    /// Delete a product variant from database by ID.
    /// </summary>
    /// <param name="id">The unique identifier of product variant.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when product variant is not found.</exception>
    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all product variants with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="ProductVariantSortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true. Defaults to false.</param>
    /// <returns>A collection of matching the product variants entities.</returns>
    Task<IEnumerable<ProductVariant>> FindAllAsync(
        CancellationToken cancellationToken,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false,
        ProductVariantSortBy sortBy = ProductVariantSortBy.Id,
        bool? isAvailable = null);
    /// <summary>
    /// Finds a product variant by ID.
    /// </summary>
    /// <param name="id">The product variant unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The product variant entity if found; overwise <c>null</c>.</returns>
    Task<ProductVariant?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a product variant by ID.
    /// </summary>
    /// <param name="id">The product variant unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The product variant entity if found;</returns>
    /// <exception cref="NotFoundException">Thrown when product variant is not found.</exception>
    Task<ProductVariant?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}