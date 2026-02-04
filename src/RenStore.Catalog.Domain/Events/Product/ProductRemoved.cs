namespace RenStore.Catalog.Domain.Events.Product;

public record ProductRemoved(
    Guid ProductId, 
    DateTimeOffset OccurredAt);