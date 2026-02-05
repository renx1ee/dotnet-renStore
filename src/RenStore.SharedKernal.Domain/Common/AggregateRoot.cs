using RenStore.SharedKernal.Domain.Entities;

namespace RenStore.SharedKernal.Domain.Common;

public abstract class AggregateRoot
{
    private readonly List<object> _domainEvents = new();
    
    public Guid Id { get; protected set; }
    public int Version { get; private set; }

    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    protected void Raise(object @event)
    {
        _domainEvents.Add(@event);
        Apply(@event);
        Version++;
    }

    protected abstract void Apply(object @event);

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
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