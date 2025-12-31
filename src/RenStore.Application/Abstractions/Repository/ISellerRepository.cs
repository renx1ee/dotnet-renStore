using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;

namespace RenStore.Domain.Repository;
/// <summary>
/// Repository for working with <see cref="SellerEntity"/>.
/// Provides basic CRUD operations and data retrieval methods with sorting and pagination.
/// </summary>
public interface ISellerRepository
{
    /// <summary>
    /// Create a new seller in the database.
    /// </summary>
    /// <param name="seller">The seller entity to create.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>ID of the created entity.</returns>
    Task<long> CreateAsync(SellerEntity seller, CancellationToken cancellationToken);
    /// <summary>
    /// Update an existing seller in the database.
    /// </summary>
    /// <param name="seller">The seller entity with updated values.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when seller is not found.</exception>
    Task UpdateAsync(SellerEntity seller, CancellationToken cancellationToken);
    /// <summary>
    /// Delete a seller by ID.
    /// </summary>
    /// <param name="id">The unique identifier of the seller.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Task representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown when seller is not found.</exception>
    Task DeleteAsync(long id, CancellationToken cancellationToken);
    /// <summary>
    /// Retrieves all sellers with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="SellerSortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <param name="isBlocked">Gets only blocked or unblocked or all seller.</param>
    /// <returns>A collection of seller entities.</returns>
    Task<IEnumerable<SellerEntity>> FindAllAsync(
        CancellationToken cancellationToken,
        SellerSortBy sortBy = SellerSortBy.Id,
        uint pageCount = 25, 
        uint page = 1,
        bool descending = false,
        bool? isBlocked = null);
    /// <summary>
    /// Finds a seller by ID.
    /// </summary>
    /// <param name="id">The seller identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The seller entity if found; otherwise, <c>null</c>.</returns>
    Task<SellerEntity?> FindByIdAsync(long id, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a seller by ID.
    /// </summary>
    /// <param name="id">The seller identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The seller entity.</returns>
    /// <exception cref="NotFoundException">Thrown when seller is not found.</exception>
    Task<SellerEntity> GetByIdAsync(long id, CancellationToken cancellationToken);
    /// <summary>
    /// Searches sellers by name with sorting and pagination.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="SellerSortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <param name="isBlocked">Gets only blocked or unblocked or all seller.</param>
    /// <returns>A collection of matching seller entities.</returns>
    Task<IEnumerable<SellerEntity>> FindByNameAsync(
        string name,
        CancellationToken cancellationToken,
        SellerSortBy sortBy = SellerSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false,
        bool? isBlocked = null);
    /// <summary>
    /// Searches sellers by name with sorting and pagination.
    /// </summary>
    /// <param name="name">The name to search for.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Defaults to <see cref="SellerSortBy.Id"/>.</param>
    /// <param name="pageCount">Number of items per page. Defaults to 25.</param>
    /// <param name="page">Page number (1-based). Defaults to 1.</param>
    /// <param name="descending">Sort in descending order if true.</param>
    /// <returns>A collection of matching seller entities.</returns>
    /// <param name="isBlocked">Gets only blocked or unblocked or all seller.</param>
    /// <exception cref="NotFoundException">Thrown when seller is not found.</exception>
    Task<IEnumerable<SellerEntity>> GetByNameAsync(string name,
        CancellationToken cancellationToken,
        SellerSortBy sortBy = SellerSortBy.Id,
        uint pageCount = 25,
        uint page = 1,
        bool descending = false,
        bool? isBlocked = null);
    /// <summary>
    /// Finds a seller by user ID.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The seller entity if found; otherwise, <c>null</c>.</returns>
    Task<SellerEntity?> FindByUserIdAsync(string userId, CancellationToken cancellationToken);
    /// <summary>
    /// Gets a seller by user ID.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>The seller entity</returns>
    /// <exception cref="NotFoundException">Thrown when seller is not found.</exception>
    Task<SellerEntity> GetByUserIdAsync(string userId, CancellationToken cancellationToken);
}