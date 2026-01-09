using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository for working with <see cref="SellerImageEntity"/>.
/// Provide basic CRUD operations and data retrieval methods with sorting and pagination.
/// </summary>
public interface ISellerImageRepository
{
    /// <summary>
    /// Create a new image in the database.
    /// </summary>
    /// <param name="image">The image entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>ID of the created entity.</returns>
    Task<Guid> CreateAsync(SellerImageEntity image, CancellationToken cancellationToken);
    /// <summary>
    /// Edit an existing image in the database.
    /// </summary>
    /// <param name="image">The image entity with the updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when image is not found.</exception>
    Task UpdateAsync(SellerImageEntity image, CancellationToken cancellationToken);
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
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="SellerImageSortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of image entities.</returns>
    Task<IEnumerable<SellerImageEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false,
        SellerImageSortBy sortBy = SellerImageSortBy.Id);
    /// <summary>
    /// Finds an image by ID.
    /// </summary>
    /// <param name="id">The image identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The image entity if found; otherwise, <c>null</c>.</returns>
    Task<SellerImageEntity?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    /// <summary>
    /// Finds an image by ID.
    /// </summary>
    /// <param name="id">The image identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The image entity if found;</returns>
    /// <exception cref="NotFoundException">Thrown when image is not found.</exception>
    Task<SellerImageEntity> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}