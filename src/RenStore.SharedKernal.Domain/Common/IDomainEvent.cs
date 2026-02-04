namespace RenStore.SharedKernal.Domain.Common;

/// <summary>
/// Represents common interface for Domain Events.
/// </summary>
public interface IDomainEvent
{
    Guid AggregateId { get; }
    long Version { get; }
    DateTimeOffset OccurredAt { get; }
}