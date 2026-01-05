using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Interfaces;

/// <summary>
/// Query for working with <see cref="IDeliveryTrackingQuery"/>.
/// Provide basic read method with sorting and pagination.
/// </summary>
public interface IDeliveryTrackingQuery
{
    /// <summary>
    /// Retrieves all delivery tracking with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Default value is <see cref="DeliveryTrackingSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max value is 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted delivery tracking. Default is <see cref="null"/>.</param>
    /// <returns>A collection of matching <see cref="DeliveryTrackingReadModel"/>.</returns>
    Task<IReadOnlyList<DeliveryTrackingReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        bool descending = false,
        bool? isDeleted = null);

    /// <summary>
    /// Get a <see cref="DeliveryTrackingReadModel"/> by delivery tracking ID.
    /// </summary>
    /// <param name="id">Delivery tracking unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Return a <see cref="DeliveryTrackingReadModel"/> if found.</returns>
    Task<DeliveryTrackingReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Get a <see cref="DeliveryTrackingReadModel"/> by delivery tracking ID.
    /// </summary>
    /// <param name="id">Delivery tracking unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Return a <see cref="DeliveryTrackingReadModel"/> if found.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If delivery tracking not found.</exception>
    Task<DeliveryTrackingReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Finds a <see cref="DeliveryTrackingReadModel"/> by user ID.
    /// </summary>
    /// <param name="deliveryOrderId">User unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default is <see cref="DeliveryTrackingSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default is 1.</param>
    /// <param name="pageSize">Number of items per page. Default is 25, Max 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default is false.</param>
    /// <param name="isDeleted">Include deleted delivery tracking. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="DeliveryTrackingReadModel"/>.</returns>
    Task<IReadOnlyList<DeliveryTrackingReadModel>> FindByDeliveryOrderAsync(
        Guid deliveryOrderId,
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        bool descending = false,
        bool? isDeleted = null);

    /// <summary>
    /// Finds a <see cref="DeliveryTrackingReadModel"/> by user ID.
    /// </summary>
    /// <param name="deliveryOrderId">User unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default is <see cref="DeliveryTrackingSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default is 1.</param>
    /// <param name="pageSize">Number of items per page. Default is 25, Max 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default is false.</param>
    /// <param name="isDeleted">Include deleted delivery tracking. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="DeliveryTrackingReadModel"/>.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If delivery tracking not found.</exception>
    Task<IReadOnlyList<DeliveryTrackingReadModel>> GetByDeliveryOrderAsync(
        Guid deliveryOrderId,
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        DeliveryTrackingSortBy sortBy = DeliveryTrackingSortBy.Id,
        bool descending = false,
        bool? isDeleted = null);
}