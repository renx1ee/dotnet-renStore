using RenStore.Order.Domain.Enums;

namespace RenStore.Order.Persistence.EntityTypeConfiguration.Conversions;

internal static class OrderConversion
{
    internal static string OrderStatusToDatabase(OrderStatus status) =>
        status switch
        {
            OrderStatus.Pending   => "pending",
            OrderStatus.Confirmed => "confirmed",
            OrderStatus.Paid      => "paid",
            OrderStatus.Shipped   => "shipped",
            OrderStatus.Delivered => "delivered",
            OrderStatus.Cancelled => "cancelled",
            OrderStatus.Refunded  => "refunded",
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };

    internal static OrderStatus OrderStatusFromDatabase(string status) =>
        status switch
        {
            "pending"   => OrderStatus.Pending,
            "confirmed" => OrderStatus.Confirmed,
            "paid"      => OrderStatus.Paid,
            "shipped"   => OrderStatus.Shipped,
            "delivered" => OrderStatus.Delivered,
            "cancelled" => OrderStatus.Cancelled,
            "refunded"  => OrderStatus.Refunded,
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };

    internal static string OrderItemStatusToDatabase(OrderItemStatus status) =>
        status switch
        {
            OrderItemStatus.Active    => "active",
            OrderItemStatus.Cancelled => "cancelled",
            OrderItemStatus.Refunded  => "refunded",
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };

    internal static OrderItemStatus OrderItemStatusFromDatabase(string status) =>
        status switch
        {
            "active"    => OrderItemStatus.Active,
            "cancelled" => OrderItemStatus.Cancelled,
            "refunded"  => OrderItemStatus.Refunded,
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };
}