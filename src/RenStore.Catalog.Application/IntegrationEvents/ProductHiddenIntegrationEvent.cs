namespace RenStore.Catalog.Application.IntegrationEvents;

public sealed record ProductHiddenIntegrationEvent(
    Guid ProductId,
    DateTimeOffset OccurredAt,
    Guid UpdatedById,
    string UpdatedByRole) 
    : IIntegrationEvent,
      INotification;