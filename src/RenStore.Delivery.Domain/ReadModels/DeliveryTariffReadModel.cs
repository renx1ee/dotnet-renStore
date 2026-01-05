using RenStore.Delivery.Domain.Enums;

namespace RenStore.Delivery.Domain.ReadModels;

/// <summary>
/// Read model, represents the delivery tariff entity.
/// Used to display and transmit data without state change logic.
/// </summary>
public class DeliveryTariffReadModel
{
    public Guid Id { get; init; } 
    public decimal Price { get; init; } 
    public DeliveryTariffType Type { get; init; }
    public string Description { get; init; } = string.Empty;
    public decimal WeightLimitKg { get; init; }
    public Guid DeliveryOrderId { get; init; }
    public bool IsDeleted { get; init; }
}