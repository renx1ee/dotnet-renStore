using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Aggregates.Variant.Events;
using RenStore.Catalog.Domain.Aggregates.VariantMedia.Events;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.VariantMedia;

internal class VariantMedia 
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    private readonly List<ProductImage> _images = new();
    
    /// <summary>
    /// The collection of images associated with this variant.
    /// </summary>
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();
    
    /// <summary>
    /// Adds a product image to this variant with metadata and display rules.
    /// Images are used for visual presentation in catalog listings and product pages.
    /// </summary>
    /// <param name="now">Timestamp for creation audit</param>
    /// <param name="originalFileName">Original uploaded file name</param>
    /// <param name="storagePath">Path to stored image file</param>
    /// <param name="fileSizeBytes">Size of image file in bytes</param>
    /// <param name="isMain">Whether this is the primary display image</param>
    /// <param name="sortOrder">Display order in image gallery (lower = first)</param>
    /// <param name="weight">Image width in pixels</param>
    /// <param name="height">Image height in pixels</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Variant is deleted
    /// - Maximum image limit reached
    /// - Image metadata fails validation (size, dimensions, etc.)
    /// </exception>
    /// <remarks>
    /// Only one image can be marked as main. If <paramref name="isMain"/> is true,
    /// any existing main image will be demoted. Images are displayed according to sort order.
    /// </remarks>
    public void AddImage(
        DateTimeOffset now,
        string originalFileName,
        string storagePath,
        long fileSizeBytes,
        bool isMain,
        short sortOrder,
        int weight, 
        int height)
    {
        ProductVariantRules.MaxImagesCountValidation(_images.Count);

        var imageId = Guid.NewGuid();
        
        ProductImageRules.CreateProductImageValidation(
            imageId: imageId,
            productVariantId: Id,
            originalFileName: originalFileName,
            storagePath: storagePath,
            fileSizeBytes: fileSizeBytes,
            sortOrder: sortOrder,
            weight: weight,
            height: height);
        
        if (isMain)
        {
            var currentMain = _images.FirstOrDefault(x => x.IsMain);

            if (currentMain != null)
            {
                Raise(new VariantImageMainUnset(
                    OccurredAt: now,
                    VariantId: Id,
                    ImageId: currentMain.Id));
            }
        }
        
        Raise(new VariantImageCreated(
            OccurredAt: now,
            ImageId: imageId,
            VariantId: Id,
            OriginalFileName: originalFileName,
            StoragePath: storagePath,
            FileSizeBytes: fileSizeBytes,
            IsMain: isMain,
            SortOrder: sortOrder,
            Weight: weight,
            Height: height));
    }
    
    /// <summary>
    /// Designates a specific image as the primary display image for this variant.
    /// The main image is featured prominently in catalog listings and product pages.
    /// </summary>
    /// <param name="now">Timestamp for status change</param>
    /// <param name="imageId">Identifier of the image to promote</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Variant is deleted
    /// - Image with specified ID is not found
    /// - Image is already deleted
    /// </exception>
    /// <remarks>
    /// If another image is currently marked as main, it will be demoted automatically.
    /// No action is taken if the specified image is already the main image.
    /// Only one image can be designated as main at any time.
    /// </remarks>
    public void MarkImageAsMain(
        DateTimeOffset now,
        Guid imageId)
    {
        var existingImage = GetImageById(imageId);
        
        var currentMain = _images.FirstOrDefault(x => x.IsMain);
        
        if(currentMain?.Id == imageId)
            return;

        if (currentMain != null)
        {
            Raise(new VariantImageMainUnset(
                OccurredAt: now,
                VariantId: Id,
                ImageId: currentMain.Id));
        }
        
        Raise(new VariantImageMainSet(
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));
    }

    public void ChangeImageSortOrder(
        DateTimeOffset now,
        Guid imageId,
        short sortOrder)
    {
        var image = GetImageById(imageId);
        
        if(image.SortOrder == sortOrder) return;
        
        Raise(new ImageSortOrderUpdated(
            OccurredAt: now,
            ImageId: imageId,
            SortOrder: sortOrder));
    }

    public void ChangeImageStoragePath(
        DateTimeOffset now,
        Guid imageId,
        string storagePath)
    {
        ProductImageRules.StoragePathValidate(storagePath);
        
        var image = GetImageById(imageId);
        
        if(image.StoragePath == storagePath) return;
        
        Raise(new ImageStoragePathUpdated(
            OccurredAt: now,
            ImageId: imageId,
            StoragePath: storagePath));
    }

    public void ChangeImageDimension(
        DateTimeOffset now,
        Guid imageId,
        int weight,
        int height)
    {
        ProductImageRules.WeightAndHeightValidate(
            weight: weight, 
            height: height);
        
        var image = GetImageById(imageId);
        
        if(image.Weight == weight && image.Height == height) 
            return;
        
        Raise(new ImageDimensionChanged(
            OccurredAt: now,
            ImageId: imageId,
            Weight: weight,
            Height: height));
    }

    public void ChangeImageFileSizeBytes(
        Guid imageId,
        DateTimeOffset now,
        long fileSizeBytes)
    {
        ProductImageRules.FileSizeBytesValidate(fileSizeBytes);
        
        var image = GetImageById(imageId);
        
        if(image.FileSizeBytes == fileSizeBytes) 
            return;
        
        Raise(new ImageFileSizeBytesChanged(
            OccurredAt: now,
            ImageId: imageId,
            FileSizeBytes: fileSizeBytes));
    }
    
    /// <summary>
    /// Marks a product image as deleted, removing it from display in the catalog.
    /// Implements soft deletion for potential recovery and audit trail.
    /// </summary>
    /// <param name="now">Timestamp for deletion audit</param>
    /// <param name="imageId">Identifier of the image to delete</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Image with specified ID is not found
    /// - Image is already marked as deleted
    /// </exception>
    /// <remarks>
    /// If the deleted image was marked as main, the main image status should be
    /// reassigned to another available image or cleared.
    /// Deleted images may be retained in storage for compliance or recovery purposes.
    /// </remarks>
    public void DeleteImage(
        DateTimeOffset now,
        Guid imageId)
    {
        var existingImage = GetImageById(imageId);
        
        Raise(new VariantImageRemoved(
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));
    }
    
    /// <summary>
    /// Restores a previously deleted product image, making it visible in the catalog again.
    /// Reverses the effect of <see cref="DeleteImage"/> while preserving the image's metadata.
    /// </summary>
    /// <param name="now">Timestamp for restoration audit</param>
    /// <param name="imageId">Identifier of the image to restore</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Image with specified ID is not found
    /// - Image is not currently marked as deleted
    /// </exception>
    /// <remarks>
    /// Restored images regain their previous display position and properties.
    /// The image file must still exist in storage for successful restoration.
    /// Restoration does not automatically re-mark the image as main if it was previously.
    /// </remarks>
    public void RestoreImage(
        DateTimeOffset now,
        Guid imageId)
    {
        var existingImage = GetImageById(imageId);
        
        Raise(new VariantImageRestored(
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));
    }
    
    protected override void Apply(object @event)
    {
        switch (@event)
        {
            case VariantImageCreated e:
                _images.Add(ProductImage.Create(
                    now: e.OccurredAt,
                    imageId: e.ImageId,
                    productVariantId: e.VariantId,
                    originalFileName: e.OriginalFileName,
                    storagePath: e.StoragePath,
                    fileSizeBytes: e.FileSizeBytes,
                    isMain: e.IsMain,
                    sortOrder: e.SortOrder,
                    weight: e.Weight,
                    height: e.Height));
                break;
            
            case ImageSortOrderUpdated e:
                _images.Single(x => x.Id == e.ImageId)
                    .ChangeSortOrder(
                        now: e.OccurredAt,
                        sortOrder: e.SortOrder);
                break;
            
            case ImageStoragePathUpdated e:
                _images.Single(x => x.Id == e.ImageId)
                    .ChangeStoragePath(
                        now: e.OccurredAt,
                        storagePath: e.StoragePath);
                break;
            
            case ImageDimensionChanged e:
                _images.Single(x => x.Id == e.ImageId)
                    .ChangeDimension(
                        now: e.OccurredAt,
                        weight: e.Weight,
                        height: e.Height);
                break;
            
            case ImageFileSizeBytesChanged e:
                _images.Single(x => x.Id == e.ImageId)
                .ChangeFileSizeBytes(
                    now: e.OccurredAt,
                    fileSizeBytes: e.FileSizeBytes);
                break;
            
            case VariantImageMainUnset e:
                _images.Single(x => x.Id == e.ImageId)
                    .UnsetAsMain(e.OccurredAt);
                break;
            
            case VariantImageMainSet e:
                _images.Single(x => x.Id == e.ImageId)
                    .SetAsMain(e.OccurredAt);
                break;
            
            case VariantImageRemoved e:
                _images
                    .Single(x => x.Id == e.ImageId)
                    .Delete(e.OccurredAt);
                break;
            
            case VariantImageRestored e:
                _images
                    .Single(x => x.Id == e.ImageId)
                    .Restore(e.OccurredAt);
                break;
        }
    }
    // TODO:
    private ProductImage GetImageById(Guid imageId)
    {
        var image = _images.SingleOrDefault(x => x.Id == imageId);
        
        if (image is null || image.IsDeleted)
            throw new DomainException("Image is not found or already was deleted.");

        return image;
    }
    
    /// <summary>
    /// Ensures the attribute is not deleted before performing operations.
    /// </summary>
    /// <param name="message">Optional custom error message</param>
    /// <exception cref="DomainException">Thrown when attribute is deleted</exception>
    private void EnsureNotDeleted(string? message = null)
    {
        /*if (IsDeleted)*/
            throw new DomainException(message ?? "Entity is deleted.");
    }
}