namespace RenStore.Order.Domain.Enums;

public enum OrderStatus
{
    /// <summary>
    /// Order has been created but not yet confirmed.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Order has been confirmed by the seller/system.
    /// </summary>
    Confirmed = 1,

    /// <summary>
    /// Payment has been received.
    /// </summary>
    Paid = 2,

    /// <summary>
    /// Order has been shipped.
    /// </summary>
    Shipped = 3,

    /// <summary>
    /// Order has been delivered to the customer.
    /// </summary>
    Delivered = 4,

    /// <summary>
    /// Order has been fully cancelled.
    /// </summary>
    Cancelled = 5,

    /// <summary>
    /// Order has been fully refunded.
    /// </summary>
    Refunded = 6
}