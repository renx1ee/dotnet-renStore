using RenStore.Order.Domain.Aggregates.Order.Events;

namespace RenStore.Order.Persistence.EventStore;

internal static class DomainEventMappings
{
    public static readonly Dictionary<string, Type> DomainEventsNameToType = new()
    {
        // Order lifecycle
        { "order-created",                    typeof(OrderCreatedEvent) },
        { "order-confirmed",                  typeof(OrderConfirmedEvent) },
        { "order-paid",                       typeof(OrderPaidEvent) },
        { "order-shipped",                    typeof(OrderShippedEvent) },
        { "order-delivered",                  typeof(OrderDeliveredEvent) },
        { "order-cancelled",                  typeof(OrderCancelledEvent) },
        { "order-refunded",                   typeof(OrderRefundedEvent) },
        { "order-shipping-address-changed",   typeof(OrderShippingAddressChangedEvent) },

        // Order items
        { "order-item-added",                 typeof(OrderItemAddedEvent) },
        { "order-item-removed",               typeof(OrderItemRemovedEvent) },
        { "order-item-quantity-changed",      typeof(OrderItemQuantityChangedEvent) },
        { "order-item-cancelled",             typeof(OrderItemCancelledEvent) },
        { "order-item-refunded",              typeof(OrderItemRefundedEvent) },
    };

    public static readonly Dictionary<Type, string> TypeToDomainEventsName = new()
    {
        // Order lifecycle
        { typeof(OrderCreatedEvent),                   "order-created" },
        { typeof(OrderConfirmedEvent),                 "order-confirmed" },
        { typeof(OrderPaidEvent),                      "order-paid" },
        { typeof(OrderShippedEvent),                   "order-shipped" },
        { typeof(OrderDeliveredEvent),                 "order-delivered" },
        { typeof(OrderCancelledEvent),                 "order-cancelled" },
        { typeof(OrderRefundedEvent),                  "order-refunded" },
        { typeof(OrderShippingAddressChangedEvent),    "order-shipping-address-changed" },

        // Order items
        { typeof(OrderItemAddedEvent),                 "order-item-added" },
        { typeof(OrderItemRemovedEvent),               "order-item-removed" },
        { typeof(OrderItemQuantityChangedEvent),       "order-item-quantity-changed" },
        { typeof(OrderItemCancelledEvent),             "order-item-cancelled" },
        { typeof(OrderItemRefundedEvent),              "order-item-refunded" },
    };

    /// <summary>
    /// Resolves the event type name for serialization.
    /// Throws if the event type is not registered — fail fast, no silent data loss.
    /// </summary>
    public static string GetEventName(Type eventType)
    {
        if (TypeToDomainEventsName.TryGetValue(eventType, out var name))
            return name;

        throw new InvalidOperationException(
            $"Domain event type '{eventType.FullName}' is not registered in {nameof(DomainEventMappings)}.");
    }

    /// <summary>
    /// Resolves the CLR type from a persisted event name.
    /// Throws if the name is unknown — prevents silent deserialization gaps.
    /// </summary>
    public static Type GetEventType(string eventName)
    {
        if (DomainEventsNameToType.TryGetValue(eventName, out var type))
            return type;

        throw new InvalidOperationException(
            $"Unknown domain event name '{eventName}'. Register it in {nameof(DomainEventMappings)}.");
    }
}