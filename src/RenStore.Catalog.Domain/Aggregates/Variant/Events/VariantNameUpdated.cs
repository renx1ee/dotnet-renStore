namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

/// <summary>
/// Records a change to the display name of a product variant.
/// Updates the customer-facing identifier used in catalog listings and search results.
/// </summary>
/// <param name="OccurredAt">Timestamp when the name was updated</param>
/// <param name="VariantId">Identifier of the renamed product variant</param>
/// <param name="Name">New display name for the variant</param>
/// <remarks>
/// Name changes may occur due to:
/// - Product rebranding
/// - Correction of errors
/// - Standardization of naming conventions
/// - SEO optimization
/// The normalized version (uppercase) is automatically regenerated for search indexing.
/// </remarks>
public record VariantNameUpdated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    string Name);