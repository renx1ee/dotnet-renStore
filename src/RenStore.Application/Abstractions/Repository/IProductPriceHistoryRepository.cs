using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository with working with <see cref="ProductPriceHistoryEntity"/>.
/// Provide basic CRUD operations and data retrieval methods with sorting and pagination.
/// </summary>
public interface IProductPriceHistoryRepository
{
    /// <summary>
    /// Create a new product price history in the database.
    /// </summary>
    /// <param name="priceHistory">The product price history for create.</param>
    /// <param name="cancellationToken">CancellationToken.</param>
    /// <returns>ID of created entity.</returns>
    Task<Guid> CreateAsync(
        ProductPriceHistoryEntity priceHistory,
        CancellationToken cancellationToken);
    /// <summary>
    /// Edit an existing product price history in the database.
    /// </summary>
    /// <param name="priceHistory">The product price history with updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when product price history is not found.</exception>
    Task UpdateAsync(
        ProductPriceHistoryEntity priceHistory,
        CancellationToken cancellationToken);
    /// <summary>
    /// Delete a product price history from database by ID.
    /// </summary>
    /// <param name="id">The unique identifier of product price history.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when product price history is not found.</exception>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all product price histories with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="ProductPriceHistorySortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true. Defaults to false.</param>
    /// <returns>A collection of matching the product price history entities.</returns>
    Task<IEnumerable<ProductPriceHistoryEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductPriceHistorySortBy sortBy = ProductPriceHistorySortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false);
    /// <summary>
    /// Finds a product price history by ID.
    /// </summary>
    /// <param name="id">The product price history unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The product price history entity if found; overwise <c>null</c>.</returns>
    Task<ProductPriceHistoryEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a product price history by ID.
    /// </summary>
    /// <param name="id">The product price history unique identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The product price history entity if found.</returns>
    /// <exception cref="NotFoundException">Thrown when product price history is not found.</exception>
    Task<ProductPriceHistoryEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}