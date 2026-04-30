using MediatR;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Payment.Application.Common;

public record DomainEventNotification<TDomainEvent>
    : INotification
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; }

    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}