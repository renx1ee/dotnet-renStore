using RenStore.Order.Domain.Enums;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Order.Domain.ReadModels;

/// <summary>
/// Read model for a single order line item.
/// </summary>
public sealed class OrderItemProjection
{
    public Guid OrderItemId { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid VariantId { get; private set; }
    public Guid SizeId { get; private set; }
    public int Quantity { get; private set; }
    public decimal PriceAmount { get; private set; }
    public Currency Currency { get; private set; }
    public string ProductNameSnapshot { get; private set; } = string.Empty;
    public OrderItemStatus Status { get; private set; }
    public string? CancellationReason { get; private set; }
    public decimal TotalPrice => PriceAmount * Quantity;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
}