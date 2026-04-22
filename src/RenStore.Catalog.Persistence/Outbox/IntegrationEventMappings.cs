using RenStore.Catalog.Application.IntegrationEvents;
using RenStore.Catalog.Contracts.Events;

namespace RenStore.Catalog.Persistence.Outbox;

public static class IntegrationEventMappings
{
    public static readonly Dictionary<string, Type> NameToType = new()
    {
        { "variant-size-created-integration", typeof(VariantSizeCreatedIntegrationEvent) },
        { "variant-size-deleted-integration", typeof(VariantSizeDeletedIntegrationEvent) },
        { "product-archived-integration",     typeof(ProductArchivedIntegrationEvent) },
        { "product-hidden-integration",       typeof(ProductHiddenIntegrationEvent) },
    };
    
    public static readonly Dictionary<Type, string> TypeToName = new()
    {
        { typeof(VariantSizeCreatedIntegrationEvent), "variant-size-created-integration" },
        { typeof(VariantSizeDeletedIntegrationEvent), "variant-size-deleted-integration" },
        { typeof(ProductArchivedIntegrationEvent),    "product-archived-integration" },
        { typeof(ProductHiddenIntegrationEvent),      "product-hidden-integration" }
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