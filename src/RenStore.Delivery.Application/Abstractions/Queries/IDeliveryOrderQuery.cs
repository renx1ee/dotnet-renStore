using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Abstractions.Queries;

public interface IDeliveryOrderQuery
{
    Task<DeliveryOrderReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<DeliveryOrderReadModel?> FindByOrderIdAsync(
        Guid orderId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<DeliveryOrderReadModel>> FindAllAsync(
        DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.CreatedAt,
        uint page = 1,
        uint pageSize = 25,
        bool descending = true,
        DeliveryStatus? status = null,
        CancellationToken cancellationToken = default);
}