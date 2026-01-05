namespace RenStore.Delivery.Domain.ReadModels;

/// <summary>
/// Read model, represents the pickup point entity.
/// Used to display and transmit data without state change logic.
/// </summary>
public class PickupPointReadModel
{
    public long Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public Guid AddressId { get; init; }
    public bool IsDeleted { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? DeletedAt { get; init; } = null;
}