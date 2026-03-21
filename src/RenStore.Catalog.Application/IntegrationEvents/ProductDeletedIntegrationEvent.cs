namespace RenStore.Catalog.Application.IntegrationEvents;

public sealed record ProductDeletedIntegrationEvent(
    Guid ProductId,
    DateTimeOffset OccurredAt,
    Guid UpdatedById,
    string UpdatedByRole) 
    : IIntegrationEvent,
      INotification;