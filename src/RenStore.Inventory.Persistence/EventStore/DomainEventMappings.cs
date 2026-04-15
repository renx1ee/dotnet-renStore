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
}