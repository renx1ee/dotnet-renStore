namespace RenStore.Order.Domain.Enums;

public enum OrderItemStatus
{
    /// <summary>
    /// Item is active in the order.
    /// </summary>
    Active = 0,

    /// <summary>
    /// Item has been cancelled individually.
    /// </summary>
    Cancelled = 1,

    /// <summary>
    /// Item has been refunded individually.
    /// </summary>
    Refunded = 2
}