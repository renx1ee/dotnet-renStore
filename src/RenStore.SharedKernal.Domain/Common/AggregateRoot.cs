namespace RenStore.SharedKernal.Domain.Common;

public abstract class AggregateRoot
{
    private readonly List<object> _uncommittedEvents = new();
    
    public Guid Id { get; protected set; }
    public int Version { get; private set; }

    public IReadOnlyCollection<object> GetUncommittedEvents() => 
        _uncommittedEvents.AsReadOnly();

    protected void Raise(object @event)
    {
        _uncommittedEvents.Add(@event);
        Apply(@event);
        Version++;
    }

    protected abstract void Apply(object @event);

    public void UncommittedEventsClear()
    {
        _uncommittedEvents.Clear();
    }
    
    protected void LoadFromHistory(IEnumerable<object> history)
    {
        foreach (var @event in history)
        {
            Apply(@event);
            Version++;
        }
    }
}