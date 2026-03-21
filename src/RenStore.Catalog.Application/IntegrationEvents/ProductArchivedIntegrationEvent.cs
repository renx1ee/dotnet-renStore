namespace RenStore.Catalog.Application.IntegrationEvents;

public sealed record ProductArchivedIntegrationEvent(
    Guid ProductId,
    DateTimeOffset OccurredAt,
    Guid UpdatedById,
    string UpdatedByRole) 
    : IIntegrationEvent,
      INotification;