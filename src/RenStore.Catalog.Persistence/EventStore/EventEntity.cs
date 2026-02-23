namespace RenStore.Catalog.Persistence.EventStore;

/// <summary>
/// The entity, represents an event in Event Store.
/// Using to store information about events that occurred in the system.
/// </summary>
public class EventEntity
{
    /// <summary>
    /// Unique event identifier in the system.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Unique aggregate ID in the system.
    /// </summary>
    public Guid AggregateId { get; set; }
    
    /// <summary>
    /// Type of aggregate.
    /// </summary>
    public string AggregateType { get; set; }
    
    /// <summary>
    /// The version of the aggregate after the event was applied.
    /// Used for optimistic locking and integrity control. 
    /// </summary>
    public int Version { get; set; }
    
    /// <summary>
    /// Type of event.
    /// </summary>
    public string EventType { get; set; }

    /// <summary>
    /// Data of the event in the JSON format.
    /// Contains basic information about occurred event.
    /// </summary>
    public string Data { get; set; } // json
    
    /// <summary>
    /// Optional: metadata about the event in the JSON format.
    /// </summary>
    public string? Payload { get; set; } // json
    
    /// <summary>
    /// The date when the event occurred.
    /// </summary>
    public DateTimeOffset OccurredAtUtc { get; set; }
}