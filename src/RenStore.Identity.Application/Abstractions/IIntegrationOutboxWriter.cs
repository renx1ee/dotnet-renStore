using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Application.Abstractions;

public interface IIntegrationOutboxWriter
{
    void Stage<T>(T integrationEvent) where T : IIntegrationEvent;
}