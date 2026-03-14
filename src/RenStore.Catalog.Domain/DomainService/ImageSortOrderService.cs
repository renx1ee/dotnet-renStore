using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.DomainService;

public class ImageSortOrderService : IImageSortOrderService
{
    public void Reorder(
        IReadOnlyCollection<VariantImage> images, 
        IReadOnlyList<Guid> orderedImageIds, 
        DateTimeOffset now)
    {
        ArgumentNullException.ThrowIfNull(images);
        ArgumentNullException.ThrowIfNull(orderedImageIds);

        if (images.Count != orderedImageIds.Count)
            throw new DomainException("Images count ids must match images count.");

        var imageMap = images.ToDictionary(x => x.Id);

        for (int i = 0; i < orderedImageIds.Count; i++)
        {
            var imageId = orderedImageIds[i];

            if (!imageMap.TryGetValue(imageId, out var image))
                throw new DomainException($"Image {imageId} not found in variant.");

            image.ChangeSortOrder(
                now: now,
                sortOrder: (i + 1));
        }
    }
}