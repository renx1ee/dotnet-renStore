namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Image;

/// <summary>
/// Records the restoration of a previously removed image to a product variant.
/// Reinstates visual content that was temporarily unavailable or incorrectly deleted.
/// </summary>
/// <param name="OccurredAt">Timestamp when the image was restored</param>
/// <param name="VariantId">Identifier of the product variant</param>
/// <param name="ImageId">Identifier of the restored image</param>
/// <remarks>
/// Used for data correction, seasonal content rotation, or rights reacquisition.
/// Restoration preserves the image's original metadata and relationships.
/// The image does not automatically regain main status upon restoration.
/// </remarks>
public record VariantImageRestored(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ImageId);