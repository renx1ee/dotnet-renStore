using RenStore.Inventory.Domain.Aggregates.Reservation.Events;
using RenStore.Inventory.Domain.Aggregates.Stock.Events;

namespace RenStore.Inventory.Persistence.EventStore;

internal static class DomainEventMappings
{
    public static readonly Dictionary<string, Type> DomainEventsNameToType = new()
    {
        // Stock
        { "stock-added",           typeof(StockAddedEvent) },
        { "stock-created",         typeof(StockCreatedEvent) },
        { "stock-sale-returned",   typeof(StockSaleReturnedEvent) },
        { "stock-set",             typeof(StockSetEvent) },
        { "stock-sold",            typeof(StockSoldEvent) },
        { "stock-written-off",     typeof(StockWrittenOffEvent) },
        
        // Reservation
        { "reservation-cancelled", typeof(VariantReservationCancelledEvent) },
        { "reservation-confirmed", typeof(VariantReservationConfirmed) },
        { "reservation-created",   typeof(VariantReservationCreatedEvent) },
        { "reservation-expired",   typeof(VariantReservationExpiredEvent) },
        { "reservation-released",  typeof(VariantReservationReleased) },
    };
    
    public static readonly Dictionary<Type, string> TypeToDomainEventsName = new()
    {
        // Stock
        { typeof(StockAddedEvent),                  "stock-added" },
        { typeof(StockCreatedEvent),                "stock-created" },
        { typeof(StockSaleReturnedEvent),           "stock-sale-returned" },
        { typeof(StockSetEvent),                    "stock-set" },
        { typeof(StockSoldEvent),                   "stock-sold" },
        { typeof(StockWrittenOffEvent),             "stock-written-off" },
        
        // Reservation
        { typeof(VariantReservationCancelledEvent), "reservation-cancelled"},
        { typeof(VariantReservationConfirmed),      "reservation-confirmed" },
        { typeof(VariantReservationCreatedEvent),   "reservation-created"},
        { typeof(VariantReservationExpiredEvent),   "reservation-expired"},
        { typeof(VariantReservationReleased),       "reservation-released"},
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