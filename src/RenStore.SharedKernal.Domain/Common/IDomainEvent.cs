namespace RenStore.SharedKernal.Domain.Common;

/// <summary>
/// Represents common interface for Domain Events.
/// </summary>
public abstract record IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTimeOffset OccurredAt { get; set; }
}