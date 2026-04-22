using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Application.Abstractions;

public interface IIntegrationOutboxWriter
{
    void Stage<T>(T integrationEvent) where T : IIntegrationEvent;
}