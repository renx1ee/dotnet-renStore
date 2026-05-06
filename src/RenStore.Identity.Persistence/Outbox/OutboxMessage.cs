namespace RenStore.Identity.Persistence.Outbox;

/// <summary>
/// Persisted envelope for a domain event that must be published to the message broker.
/// Written in the same transaction as the aggregate — guarantees at-least-once delivery.
/// </summary>
public sealed class OutboxMessage
{
    /// <summary>
    /// Unique identifier of this outbox record. Used for idempotency on the consumer side.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Logical name of the event (e.g. "order-confirmed").
    /// Maps to <see cref="DomainEventMappings"/>.
    /// </summary>
    public string EventType { get; init; } = string.Empty;

    /// <summary>
    /// Aggregate stream identifier (OrderId).
    /// Useful for filtering / debugging.
    /// </summary>
    public Guid AggregateId { get; init; }

    /// <summary>
    /// JSON-serialized domain event payload.
    /// </summary>
    public string Payload { get; init; } = string.Empty;
    
    public OutboxMessageKind Kind { get; init; }

    /// <summary>
    /// When the event occurred in the domain.
    /// </summary>
    public DateTimeOffset OccurredAt { get; init; }

    /// <summary>
    /// When this record was written to the outbox table.
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>
    /// When the worker successfully published this message.
    /// Null means it has not been processed yet.
    /// </summary>
    public DateTimeOffset? ProcessedAt { get; set; }

    /// <summary>
    /// Last error message from a failed publish attempt.
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// Number of failed publish attempts.
    /// Worker uses this to apply back-off or dead-letter after a threshold.
    /// </summary>
    public int RetryCount { get; set; }
}

public enum OutboxMessageKind
{
    Domain,
    Integration
}