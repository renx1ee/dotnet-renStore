namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Image;

/// <summary>
/// Records the removal of an image from a product variant.
/// Images may be removed due to obsolescence, quality issues, or rights management.
/// </summary>
/// <param name="OccurredAt">Timestamp when the image was removed</param>
/// <param name="VariantId">Identifier of the product variant</param>
/// <param name="ImageId">Identifier of the removed image</param>
/// <remarks>
/// Typically implements soft deletion for potential recovery and audit purposes.
/// If the removed image was designated as main, the main status should be reassigned.
/// Removal may trigger cleanup of associated storage resources.
/// </remarks>
public record VariantImageRemoved(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ImageId);