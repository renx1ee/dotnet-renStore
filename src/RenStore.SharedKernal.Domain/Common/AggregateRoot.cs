namespace RenStore.SharedKernal.Domain.Common;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _uncommittedEvents = new();
    
    public Guid Id { get; protected set; }
    public int Version { get; protected set; }

    public IReadOnlyCollection<IDomainEvent> GetUncommittedEvents() => 
        _uncommittedEvents.AsReadOnly();

    protected void Raise(IDomainEvent @event)
    {
        Apply(@event);
        _uncommittedEvents.Add(@event);
    }

    protected abstract void Apply(IDomainEvent @event);

    public void UncommittedEventsClear()
    {
        _uncommittedEvents.Clear();
    }
}