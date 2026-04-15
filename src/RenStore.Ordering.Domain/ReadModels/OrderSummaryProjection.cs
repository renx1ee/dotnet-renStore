using RenStore.Order.Domain.Enums;

namespace RenStore.Order.Domain.ReadModels;

/// <summary>
/// Lightweight read model for order listings.
/// </summary>
public sealed class OrderSummaryProjection
{
    public Guid OrderId { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public string ShippingAddress { get; private set; } = string.Empty;
    public string? TrackingNumber { get; private set; }
    public string? CancellationReason { get; private set; }
    public decimal TotalAmount { get; private set; }
    public int TotalItemCount { get; private set; }
    public int ActiveItemCount { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
}