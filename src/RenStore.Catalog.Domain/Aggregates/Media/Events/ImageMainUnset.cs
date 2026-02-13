namespace RenStore.Catalog.Domain.Aggregates.Media.Events;

/// <summary>
/// Records when an image is no longer designated as the primary display image for a variant.
/// Used when demoting a main image or when no main image is currently set.
/// </summary>
/// <param name="OccurredAt">Timestamp when the image was unset as main</param>
/// <param name="ImageId">Identifier of the image that is no longer main</param>
/// <remarks>
/// Typically occurs when:
/// - A different image is set as the new main
/// - The main image is deleted
/// - The variant loses all images
/// 
/// The variant may remain without a main image until one is explicitly designated.
/// </remarks>
public record ImageMainUnset(
    DateTimeOffset OccurredAt,
    Guid ImageId);