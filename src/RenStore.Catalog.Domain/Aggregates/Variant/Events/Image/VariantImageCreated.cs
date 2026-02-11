namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Image;

/// <summary>
/// Records the addition of a visual asset to a product variant.
/// Images provide visual representation for catalog display and customer decision-making.
/// </summary>
/// <param name="OccurredAt">Timestamp when the image was added</param>
/// <param name="ImageId">Unique identifier for the image record</param>
/// <param name="VariantId">Identifier of the product variant</param>
/// <param name="OriginalFileName">Original uploaded file name</param>
/// <param name="StoragePath">Path to stored image file</param>
/// <param name="FileSizeBytes">Size of the image file in bytes</param>
/// <param name="IsMain">Whether this is the primary display image</param>
/// <param name="SortOrder">Display sequence in the image gallery</param>
/// <param name="Weight">Image width in pixels</param>
/// <param name="Height">Image height in pixels</param>
/// <remarks>
/// Multiple images can be added per variant, with only one marked as main.
/// SortOrder determines display sequence (lower numbers appear first).
/// Images are required before a variant can be published to the catalog.
/// </remarks>
public record VariantImageCreated(
    DateTimeOffset OccurredAt,
    Guid ImageId,
    Guid VariantId,
    string OriginalFileName,
    string StoragePath,
    long FileSizeBytes,
    bool IsMain,
    short SortOrder,
    int Weight, 
    int Height);