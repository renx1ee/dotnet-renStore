using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Interfaces;
/// <summary>
/// Query for working with <see cref="DeliveryTariffReadModel"/>.
/// Provide basic read method with sorting and pagination.
/// </summary>
public interface IDeliveryTariffQuery
{
    /// <summary>
    /// Retrieves all delivery tariffs with sorting and pagination.
    /// </summary>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <param name="sortBy">Fields to sort by. Default value is <see cref="DeliveryTariffSortBy.Id"/>.</param>
    /// <param name="page">Page number (1-based). Default value is 1.</param>
    /// <param name="pageSize">Number of items per page. Default value is 25, Max value is 1000.</param>
    /// <param name="descending">Sort in descending order if true. Default value is false.</param>
    /// <param name="isDeleted">Include deleted delivery tariffs. Default is <see cref="null"/>.</param>
    /// <returns>A collection of matching <see cref="DeliveryTariffReadModel"/>.</returns>
    Task<IReadOnlyList<DeliveryTariffReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        DeliveryTariffSortBy sortBy = DeliveryTariffSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);

    /// <summary>
    /// Get a <see cref="DeliveryTariffReadModel"/> by delivery tariff ID.
    /// </summary>
    /// <param name="id">Delivery tariff unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Return a <see cref="DeliveryTariffReadModel"/> if found.</returns>
    Task<DeliveryTariffReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Get a <see cref="DeliveryTariffReadModel"/> by delivery tariff ID.
    /// </summary>
    /// <param name="id">Delivery tariff unique identifier (ID).</param>
    /// <param name="cancellationToken">Cancellation Token.</param>
    /// <returns>Return a <see cref="DeliveryTariffReadModel"/> if found.</returns>
    /// <exception cref="RenStore.SharedKernal.Domain.Exceptions.NotFoundException">If delivery tariff not found.</exception>
    Task<DeliveryTariffReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);
}