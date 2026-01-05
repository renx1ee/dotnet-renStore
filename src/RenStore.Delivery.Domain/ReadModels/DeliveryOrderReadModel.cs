using RenStore.Delivery.Domain.Enums;

namespace RenStore.Delivery.Domain.ReadModels;

/// <summary>
/// Read model, represents the delivery order entity.
/// Used to display and transmit data without state change logic.
/// </summary>
public class DeliveryOrderReadModel
{
    public Guid Id { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? DeliveredAt { get; init; } = null;
    public DateTimeOffset? DeletedAt { get; init; } = null;
    public DeliveryStatus Status { get; init; }
    public Guid OrderId { get; init; }
    public Guid AddressId { get; init; }
    public Guid DeliveryTariffId { get; init; }
    public long? CurrentSortingCenterId { get; init; }
    public long? DestinationSortingCenterId { get; init; }
    public long? PickUpPointId { get; init; }
}