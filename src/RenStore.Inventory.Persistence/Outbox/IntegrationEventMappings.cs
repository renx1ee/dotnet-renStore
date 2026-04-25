using RenStore.Inventory.Contracts.Events;

namespace RenStore.Inventory.Persistence.Outbox;

public static class IntegrationEventMappings
{
    public static readonly Dictionary<string, Type> NameToType = new()
    {
        { "inventory-stock-availability-changed-integration", typeof(StockAvailabilityChangedIntegrationEvent) },
    };
    
    public static readonly Dictionary<Type, string> TypeToName = new()
    {
        { typeof(StockAvailabilityChangedIntegrationEvent), "inventory-stock-availability-changed-integration" },
    };

    /// <summary>
    /// Resolves the event type name for serialization.
    /// Throws if the event type is not registered — fail fast, no silent data loss.
    /// </summary>
    public static string GetEventName(Type eventType)
    {
        if (TypeToName.TryGetValue(eventType, out var name))
            return name;

        throw new InvalidOperationException(
            $"Domain event type '{eventType.FullName}' is not registered in {nameof(TypeToName)}.");
    }
    
    /// <summary>
    /// Resolves the CLR type from a persisted event name.
    /// Throws if the name is unknown — prevents silent deserialization gaps.
    /// </summary>
    public static Type GetEventType(string eventName)
    {
        if (NameToType.TryGetValue(eventName, out var type))
            return type;
        
        throw new InvalidOperationException(
            $"Unknown domain event name '{eventName}'. Register it in {nameof(NameToType)}.");
    }
}