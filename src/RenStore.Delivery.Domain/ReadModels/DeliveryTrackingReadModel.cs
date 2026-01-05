using RenStore.Delivery.Domain.Enums;

namespace RenStore.Delivery.Domain.ReadModels;

/// <summary>
/// Read model, represents the delivery tracking entity.
/// Used to display and transmit data without state change logic.
/// </summary>
public class DeliveryTrackingReadModel
{
    public Guid Id { get; init; }
    public string CurrentLocation { get; init; } = string.Empty;
    public DeliveryStatus Status { get; init; }
    public string Notes { get; init; } = string.Empty;
    public bool IsDeleted { get; init; }
    public DateTimeOffset OccurredAt { get; init; }
    public DateTimeOffset DeletedAt { get; init; }
    public long? SortingCenterId { get; init; }
    public Guid DeliveryOrderId { get; init; }
}