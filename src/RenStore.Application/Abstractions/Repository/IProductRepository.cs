using RenStore.Catalog.Domain.Aggregates.Product;
using RenStore.Catalog.Domain.DTOs.Product.FullPage;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository with working with <see cref="Product"/>.
/// Provide basic CRUD operations and data retrieval methods with sorting and pagination.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Create a new product in the database.
    /// </summary>
    /// <param name="product">The product for create.</param>
    /// <param name="cancellationToken">CancellationToken.</param>
    /// <returns>ID of created entity.</returns>
    Task<Guid> CreateAsync(
        Product product,
        CancellationToken cancellationToken);
    /// <summary>
    /// Edit an existing product in the database.
    /// </summary>
    /// <param name="product">The product with updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when product is not found.</exception>
    Task UpdateAsync(
        Product product,
        CancellationToken cancellationToken);
    /// <summary>
    /// Delete a product from database by ID.
    /// </summary>
    /// <param name="id">The unique identifier of product.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when product is not found.</exception>
    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all products with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true. Defaults to false.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="ProductSortBy.Id"/>.</param>
    /// <param name="isBlocked"></param>
    /// <returns>A collection of matching the products entities.</returns>
    Task<IEnumerable<Product>> FindAllAsync(CancellationToken cancellationToken,
        uint pageCount = 25U,
        uint page = 1U,
        bool descending = false,
        ProductSortBy sortBy = ProductSortBy.Id, 
        bool? isBlocked = null);
    /// <summary>
    /// Finds a product by ID.
    /// </summary>
    /// <param name="id">The product unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The product entity if found; overwise <c>null</c>.</returns>
    Task<Product?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a product by ID.
    /// </summary>
    /// <param name="id">The product unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The product entity if found.</returns>
    /// <exception cref="NotFoundException">Thrown when product is not found.</exception>
    Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Get a full product page by ID.
    /// </summary>
    /// <param name="id">Product unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The <see cref="ProductFullDto"/> if entity if found.</returns>
    Task<ProductFullDto?> FindFullAsync(Guid id, CancellationToken cancellationToken);
}