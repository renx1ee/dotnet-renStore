using RenStore.Order.Domain.Enums;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Order.Domain.Aggregates.Order;

/// <summary>
/// Represents a single line item within an order.
/// Stores a price and product name snapshot at the moment of order placement.
/// </summary>
public sealed class OrderItem
{
    /// <summary>
    /// Unique identifier of this order item.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Identifier of the order this item belongs to.
    /// </summary>
    public Guid OrderId { get; private set; }

    /// <summary>
    /// Identifier of the product variant selected.
    /// </summary>
    public Guid VariantId { get; private set; }

    /// <summary>
    /// Identifier of the specific size selected for this variant.
    /// </summary>
    public Guid SizeId { get; private set; }

    /// <summary>
    /// Number of units ordered.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Price per unit at the moment the order was placed (snapshot).
    /// </summary>
    public decimal PriceAmount { get; private set; }

    /// <summary>
    /// Currency of the price snapshot.
    /// </summary>
    public Currency Currency { get; private set; }

    /// <summary>
    /// Product variant name at the moment the order was placed (snapshot).
    /// Preserved in case the original product name changes later.
    /// </summary>
    public string ProductNameSnapshot { get; private set; }

    /// <summary>
    /// Current lifecycle status of this item.
    /// </summary>
    public OrderItemStatus Status { get; private set; }

    /// <summary>
    /// Reason for cancellation or refund, if applicable.
    /// </summary>
    public string? CancellationReason { get; private set; }

    /// <summary>
    /// Date when the item was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Date when the item status was last changed.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }

    private OrderItem() { }

    internal static OrderItem Create(
        Guid id,
        Guid orderId,
        Guid variantId,
        Guid sizeId,
        int quantity,
        decimal priceAmount,
        Currency currency,
        string productNameSnapshot,
        DateTimeOffset now)
    {
        return new OrderItem
        {
            Id = id,
            OrderId = orderId,
            VariantId = variantId,
            SizeId = sizeId,
            Quantity = quantity,
            PriceAmount = priceAmount,
            Currency = currency,
            ProductNameSnapshot = productNameSnapshot,
            Status = OrderItemStatus.Active,
            CreatedAt = now
        };
    }

    internal void ChangeQuantity(
        int quantity, 
        DateTimeOffset now)
    {
        Quantity = quantity;
        UpdatedAt = now;
    }

    internal void Cancel(
        string reason, 
        DateTimeOffset now)
    {
        Status = OrderItemStatus.Cancelled;
        CancellationReason = reason;
        UpdatedAt = now;
    }

    internal void Refund(
        string reason, 
        DateTimeOffset now)
    {
        Status = OrderItemStatus.Refunded;
        CancellationReason = reason;
        UpdatedAt = now;
    }

    /// <summary>
    /// Total price for this line item.
    /// </summary>
    public decimal TotalPrice => PriceAmount * Quantity;

    public bool IsActive => Status == OrderItemStatus.Active;
}