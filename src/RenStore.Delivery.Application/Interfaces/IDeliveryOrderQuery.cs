using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Interfaces;
/// <summary>
/// Query for working with <see cref="DeliveryOrderReadModel"/>.
/// Provide basic read method with sorting and pagination.
/// </summary>
public interface IDeliveryOrderQuery
{
    /// <summary>
    /// Retrieves all delivery orders with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Default value is <see cref="DeliveryOrderSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max value is 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted delivery orders. Default is <see cref="null"/>.</param>
    /// <returns>A collection of matching <see cref="DeliveryOrderReadModel"/>.</returns>
    Task<IReadOnlyList<DeliveryOrderReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);
    
    /// <summary>
    /// Finds a <see cref="DeliveryOrderReadModel"/> by delivery order ID.
    /// </summary>
    /// <param name="id">Delivery Order unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Return a <see cref="DeliveryOrderReadModel"/> if found.</returns>
    Task<DeliveryOrderReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Finds a <see cref="DeliveryOrderReadModel"/> by delivery order ID.
    /// </summary>
    /// <param name="id">Delivery Order unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Return a <see cref="DeliveryOrderReadModel"/> if found.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If delivery order not found.</exception>
    Task<DeliveryOrderReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Finds a <see cref="DeliveryOrderReadModel"/> by order ID.
    /// </summary>
    /// <param name="orderId">Order unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default is <see cref="DeliveryOrderSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default is 1.</param>
    /// <param name="pageSize">Number of items per page. Default is 25, Max 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default is false.</param>
    /// <param name="isDeleted">Include deleted delivery orders. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="DeliveryOrderReadModel"/>.</returns>
    Task<IReadOnlyList<DeliveryOrderReadModel>> FindByOrderId(
        Guid orderId,
        CancellationToken cancellationToken,
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);

    /// <summary>
    /// Gets a <see cref="DeliveryOrderReadModel"/> by order ID.
    /// </summary>
    /// <param name="orderId">Order unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default is <see cref="DeliveryOrderSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default is 1.</param>
    /// <param name="pageSize">Number of items per page. Default is 25, Max 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default is false.</param>
    /// <param name="isDeleted">Include deleted delivery orders. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="DeliveryOrderReadModel"/>.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If delivery order not found.</exception>
    Task<IReadOnlyList<DeliveryOrderReadModel>> GetByOrderId(
        Guid orderId,
        CancellationToken cancellationToken,
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);

    /// <summary>
    /// Gets a <see cref="DeliveryOrderReadModel"/> by tariff ID.
    /// </summary>
    /// <param name="tariffId">Tariff unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default is <see cref="DeliveryOrderSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default is 1.</param>
    /// <param name="pageSize">Number of items per page. Default is 25, Max 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default is false.</param>
    /// <param name="isDeleted">Include deleted delivery orders. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="DeliveryOrderReadModel"/>.</returns>
    Task<IReadOnlyList<DeliveryOrderReadModel>> FindByDeliveryTariffId(
        Guid tariffId,
        CancellationToken cancellationToken,
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);

    /// <summary>
    /// Gets a <see cref="DeliveryOrderReadModel"/> by tariff ID.
    /// </summary>
    /// <param name="tariffId">Tariff unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Field to sort by. Default is <see cref="DeliveryOrderSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default is 1.</param>
    /// <param name="pageSize">Number of items per page. Default is 25, Max 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default is false.</param>
    /// <param name="isDeleted">Include deleted delivery orders. Default is <see cref="null"/>.</param>
    /// <returns>Collection of items matching <see cref="DeliveryOrderReadModel"/>.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If delivery order not found.</exception>
    Task<IReadOnlyList<DeliveryOrderReadModel>> GetByDeliveryTariffId(
        Guid tariffId,
        CancellationToken cancellationToken,
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);

}