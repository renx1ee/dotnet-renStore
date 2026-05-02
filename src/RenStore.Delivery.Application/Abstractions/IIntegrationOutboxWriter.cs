using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Delivery.Application.Abstractions;

public interface IIntegrationOutboxWriter
{
    void Stage<T>(T integrationEvent) where T : IIntegrationEvent;
}