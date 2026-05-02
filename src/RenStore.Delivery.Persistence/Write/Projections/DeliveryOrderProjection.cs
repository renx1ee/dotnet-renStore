using Microsoft.EntityFrameworkCore;
using RenStore.Delivery.Application.Abstractions.Projections;
using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Persistence.Write.Projections;

internal sealed class DeliveryOrderProjection(
    DeliveryDbContext context)
    : IDeliveryOrderProjection
{
    public async Task CommitAsync(CancellationToken cancellationToken)
        => await context.SaveChangesAsync(cancellationToken);

    public async Task AddAsync(
        DeliveryOrderReadModel order,
        CancellationToken      cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(order);
        await context.DeliveryOrders.AddAsync(order, cancellationToken);
    }

    public async Task UpdateStatusAsync(
        DateTimeOffset now,
        Guid           deliveryOrderId,
        DeliveryStatus status,
        CancellationToken cancellationToken)
    {
        var order = await GetAsync(deliveryOrderId, cancellationToken);
        order.Status = status;
    }

    public async Task SetSortingCenterAsync(
        DateTimeOffset now,
        Guid           deliveryOrderId,
        long?          currentSortingCenterId,
        long?          destinationSortingCenterId,
        CancellationToken cancellationToken)
    {
        var order = await GetAsync(deliveryOrderId, cancellationToken);
        order.CurrentSortingCenterId     = currentSortingCenterId;
        order.DestinationSortingCenterId = destinationSortingCenterId;
        order.Status                     = currentSortingCenterId.HasValue
            ? DeliveryStatus.ArrivedAtSortingCenter
            : DeliveryStatus.EnRouteToSortingCenter;
    }

    public async Task SetPickupPointAsync(
        DateTimeOffset now,
        Guid           deliveryOrderId,
        long           pickupPointId,
        CancellationToken cancellationToken)
    {
        var order = await GetAsync(deliveryOrderId, cancellationToken);
        order.PickupPointId = pickupPointId;
        order.Status        = DeliveryStatus.EnRouteToPickupPoint;
    }

    public async Task SetDeliveredAsync(
        DateTimeOffset now,
        Guid           deliveryOrderId,
        CancellationToken cancellationToken)
    {
        var order = await GetAsync(deliveryOrderId, cancellationToken);
        order.Status      = DeliveryStatus.Delivered;
        order.DeliveredAt = now;
    }

    public async Task SetDeletedAsync(
        DateTimeOffset now,
        Guid           deliveryOrderId,
        CancellationToken cancellationToken)
    {
        var order = await GetAsync(deliveryOrderId, cancellationToken);
        order.Status    = DeliveryStatus.IsDeleted;
        order.DeletedAt = now;
    }

    private async Task<DeliveryOrderReadModel> GetAsync(
        Guid              id,
        CancellationToken cancellationToken)
    {
        return await context.DeliveryOrders
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new NotFoundException(typeof(DeliveryOrderReadModel), id);
    }
}