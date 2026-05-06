using MediatR;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Application.Common;

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