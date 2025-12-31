using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository for working with <see cref="ProductImageEntity"/>.
/// Provide basic CRUD operations and data retrieval methods with sorting and pagination.
/// </summary>
public interface IProductImageRepository
{
    /// <summary>
    /// Create a new image in the database.
    /// </summary>
    /// <param name="image">The image entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>ID of the created entity.</returns>
    Task<Guid> CreateAsync(ProductImageEntity image, CancellationToken cancellationToken);
    /// <summary>
    /// Update an existing image in the database.
    /// </summary>
    /// <param name="image">The image entity with the updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when image is not found.</exception>
    Task UpdateAsync(ProductImageEntity image, CancellationToken cancellationToken);
    /// <summary>
    /// Delete a image by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the image.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when image is not found.</exception>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all images with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="ProductImageSortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of image entities.</returns>
    Task<IEnumerable<ProductImageEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false,
        ProductImageSortBy sortBy = ProductImageSortBy.Id);
    /// <summary>
    /// Finds an image by ID.
    /// </summary>
    /// <param name="id">The image identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The image entity if found; otherwise, <c>null</c>.</returns>
    Task<ProductImageEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Finds an image by ID.
    /// </summary>
    /// <param name="id">The image identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The image entity if found;</returns>
    /// <exception cref="NotFoundException">Thrown when image is not found.</exception>
    Task<ProductImageEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}