namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

/// <summary>
/// Records the designation of an image as the primary display image for a product variant.
/// The main image is featured prominently in catalog listings and product pages.
/// </summary>
/// <param name="OccurredAt">Timestamp when the image was set as main</param>
/// <param name="VariantId">Identifier of the product variant</param>
/// <param name="ImageId">Identifier of the image designated as main</param>
/// <remarks>
/// Only one image can be marked as main at any time per variant.
/// Setting a new main image automatically demotes any previously set main image.
/// Main images are used as thumbnails and featured visuals across the platform.
/// </remarks>
public record VariantImageMainSet(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ImageId);