using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository with working with <see cref="ProductAttribute"/>.
/// Provide basic CRUD operations and data retrieval methods with sorting and pagination.
/// </summary>
public interface IProductAttributeRepository
{
    /// <summary>
    /// Create a new product attribute in the database.
    /// </summary>
    /// <param name="attribute">The product attribute for create.</param>
    /// <param name="cancellationToken">CancellationToken.</param>
    /// <returns>ID of created entity.</returns>
    Task<Guid> CreateAsync(
        ProductAttribute attribute,
        CancellationToken cancellationToken);
    /// <summary>
    /// Edit an existing product attribute in the database.
    /// </summary>
    /// <param name="attribute">The product attribute with updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when product attribute is not found.</exception>
    Task UpdateAsync(
        ProductAttribute attribute,
        CancellationToken cancellationToken);
    /// <summary>
    /// Delete a product attribute from database by ID.
    /// </summary>
    /// <param name="id">The unique identifier of product attribute.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when product attribute is not found.</exception>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all product attributes with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="ProductAttributeSortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true. Defaults to false.</param>
    /// <returns>A collection of matching the product attribute entities.</returns>
    Task<IEnumerable<ProductAttribute>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductAttributeSortBy sortBy = ProductAttributeSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Finds a product attribute by ID.
    /// </summary>
    /// <param name="id">The product attribute unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The product attribute entity if found; overwise <c>null</c>.</returns>
    Task<ProductAttribute?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a product attribute by ID.
    /// </summary>
    /// <param name="id">The product attribute unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The product attribute entity if found.</returns>
    /// <exception cref="NotFoundException">Thrown when product attribute is not found.</exception>
    Task<ProductAttribute?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}