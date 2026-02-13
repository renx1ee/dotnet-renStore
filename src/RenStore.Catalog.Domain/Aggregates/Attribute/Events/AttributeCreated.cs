namespace RenStore.Catalog.Domain.Aggregates.Attribute.Events;

/// <summary>
/// Records the addition of a descriptive attribute to a product variant.
/// Attributes provide detailed specifications that help customers evaluate the product.
/// </summary>
/// <param name="OccurredAt">Timestamp when the attribute was added</param>
/// <param name="VariantId">Identifier of the product variant receiving the attribute</param>
/// <param name="Key">Attribute category or name (e.g., "Material", "Screen Size")</param>
/// <param name="Value">Attribute specification (e.g., "Leather", "6.1 inches")</param>
/// <remarks>
/// Attributes are used for filtering, comparison, and detailed product specifications.
/// Keys and values are normalized and validated before event creation.
/// </remarks>
public record AttributeCreated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid AttributeId,
    string Key,
    string Value);