namespace RenStore.Catalog.Domain.Events.Product;

public record ProductPublished(
    Guid ProductId,
    DateTimeOffset OccurredAt);