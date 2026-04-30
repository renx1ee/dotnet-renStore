using RenStore.Payment.Domain.Aggregates.Payment.Events;

namespace RenStore.Payment.Persistence.EventStore;

internal static class DomainEventMappings
{
    public static readonly Dictionary<string, Type> DomainEventsNameToType = new()
    {
        // Payment lifecycle
        { "payment-created",                  typeof(PaymentCreatedEvent) },
        { "payment-authorized",               typeof(PaymentAuthorizedEvent) },
        { "payment-captured",                 typeof(PaymentCapturedEvent) },
        { "payment-failed",                   typeof(PaymentFailedEvent) },
        { "payment-cancelled",                typeof(PaymentCancelledEvent) },
        { "payment-expired",                  typeof(PaymentExpiredEvent) },

        // Payment attempts
        { "payment-attempt-created",          typeof(PaymentAttemptCreatedEvent) },

        // Refund events
        { "refund-initiated",                 typeof(RefundInitiatedEvent) },
        { "refund-succeeded",                 typeof(RefundSucceededEvent) },
        { "refund-failed",                    typeof(RefundFailedEvent) },
    };

    public static readonly Dictionary<Type, string> TypeToDomainEventsName = new()
    {
        // Payment lifecycle
        { typeof(PaymentCreatedEvent),                "payment-created" },
        { typeof(PaymentAuthorizedEvent),             "payment-authorized" },
        { typeof(PaymentCapturedEvent),               "payment-captured" },
        { typeof(PaymentFailedEvent),                 "payment-failed" },
        { typeof(PaymentCancelledEvent),              "payment-cancelled" },
        { typeof(PaymentExpiredEvent),                "payment-expired" },

        // Payment attempts
        { typeof(PaymentAttemptCreatedEvent),         "payment-attempt-created" },

        // Refund events
        { typeof(RefundInitiatedEvent),               "refund-initiated" },
        { typeof(RefundSucceededEvent),               "refund-succeeded" },
        { typeof(RefundFailedEvent),                  "refund-failed" },
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