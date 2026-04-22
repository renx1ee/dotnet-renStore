namespace RenStore.Catalog.Application.Abstractions;

public interface IIntegrationOutboxWriter
{
    void Stage<T>(T integrationEvent) where T : IIntegrationEvent;
}