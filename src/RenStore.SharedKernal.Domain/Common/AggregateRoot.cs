namespace RenStore.SharedKernal.Domain.Common;

public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _uncommittedEvents = new();
    
    public Guid Id { get; protected set; }
    public int Version { get; private set; }

    public IReadOnlyCollection<IDomainEvent> GetUncommittedEvents() => 
        _uncommittedEvents.AsReadOnly();

    protected void Raise(IDomainEvent @event)
    {
        _uncommittedEvents.Add(@event);
        Apply(@event);
        Version++;
    }

    protected abstract void Apply(IDomainEvent @event);

    public void UncommittedEventsClear()
    {
        _uncommittedEvents.Clear();
    }
    
    protected void LoadFromHistory(IEnumerable<IDomainEvent> history)
    {
        foreach (var @event in history)
        {
            Apply(@event);
            Version++;
        }
    }
}