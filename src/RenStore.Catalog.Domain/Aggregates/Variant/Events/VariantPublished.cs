namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

/// <summary>
/// Records when a product variant becomes publicly available in the catalog.
/// Marks the completion of variant setup and enables customer purchases.
/// </summary>
/// <param name="VariantId">Identifier of the published product variant</param>
/// <param name="OccurredAt">Timestamp when publication occurred</param>
/// <remarks>
/// Publication requires:
/// - At least one product image
/// - Complete product details
/// - Valid inventory levels
/// Published variants appear in search results and are available for order placement.
/// This is a significant business event that may trigger notifications and indexing updates.
/// </remarks>
public record VariantPublished(
    Guid VariantId,
    DateTimeOffset OccurredAt);