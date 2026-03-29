using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Application.Common;

public sealed record DomainEventNotification<TDomainEvent>
    : INotification
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; }

    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}