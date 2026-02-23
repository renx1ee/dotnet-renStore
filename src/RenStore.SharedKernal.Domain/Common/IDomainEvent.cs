namespace RenStore.SharedKernal.Domain.Common;

/// <summary>
/// Represents common interface for Domain Events.
/// </summary>
public interface IDomainEvent
{
    public DateTimeOffset OccurredAt { get; init; }
    public Guid EventId { get; init; }
}