using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Payment.Application.Abstractions;

public interface IIntegrationOutboxWriter
{
    void Stage<T>(T integrationEvent) where T : IIntegrationEvent;
}