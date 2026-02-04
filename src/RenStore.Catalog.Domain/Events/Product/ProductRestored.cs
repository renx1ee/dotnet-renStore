namespace RenStore.Catalog.Domain.Events.Product;

public record ProductRestored(
    Guid ProductId, 
    DateTimeOffset OccurredAt);