using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Abstractions.Projections;

public interface IDeliveryOrderProjection
{
    Task CommitAsync(CancellationToken cancellationToken);

    Task AddAsync(
        DeliveryOrderReadModel order,
        CancellationToken      cancellationToken);

    Task UpdateStatusAsync(
        DateTimeOffset now,
        Guid           deliveryOrderId,
        DeliveryStatus status,
        CancellationToken cancellationToken);

    Task SetSortingCenterAsync(
        DateTimeOffset now,
        Guid           deliveryOrderId,
        long?          currentSortingCenterId,
        long?          destinationSortingCenterId,
        CancellationToken cancellationToken);

    Task SetPickupPointAsync(
        DateTimeOffset now,
        Guid           deliveryOrderId,
        long           pickupPointId,
        CancellationToken cancellationToken);

    Task SetDeliveredAsync(
        DateTimeOffset now,
        Guid           deliveryOrderId,
        CancellationToken cancellationToken);

    Task SetDeletedAsync(
        DateTimeOffset now,
        Guid           deliveryOrderId,
        CancellationToken cancellationToken);
}