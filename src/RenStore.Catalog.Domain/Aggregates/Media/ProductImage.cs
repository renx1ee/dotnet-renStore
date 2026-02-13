using RenStore.Catalog.Domain.Aggregates.Media.Events;
using RenStore.Catalog.Domain.Aggregates.Variant.Rules;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Media;

/// <summary>
/// Represents a product image physical entity with lifecycle and invariants.
/// </summary>
public class ProductImage
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    public Guid Id { get; private set; }
    public string OriginalFileName { get; private set; }
    public string StoragePath { get; private set; }
    public long FileSizeBytes { get; private set; }
    public bool IsMain { get; private set; }
    public short SortOrder { get; private set; } 
    public int Weight { get; private set; }
    public int Height { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset UploadedAt { get; private set; } 
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    
    private ProductImage() { }
    
    internal static ProductImage Create(
        DateTimeOffset now,
        Guid variantId,
        string originalFileName,
        string storagePath,
        long fileSizeBytes,
        bool isMain,
        short sortOrder,
        int weight, 
        int height)
    {
        ProductImageRules.CreateProductImageValidation(
            productVariantId: variantId,
            originalFileName: originalFileName,
            storagePath: storagePath,
            fileSizeBytes: fileSizeBytes,
            sortOrder: sortOrder,
            weight: weight,
            height: height);

        var imageId = Guid.NewGuid();
        var image = new ProductImage();
        
        image.Raise(new ImageCreated(
            OccurredAt: now,
            ImageId: imageId,
            VariantId: variantId,
            OriginalFileName: originalFileName,
            StoragePath: storagePath,
            FileSizeBytes: fileSizeBytes,
            IsMain: isMain,
            SortOrder: sortOrder,
            Weight: weight,
            Height: height));

        return image;
    }
    
    public void ChangeSortOrder(
        DateTimeOffset now,
        short sortOrder)
    {
        EnsureNotDeleted();
        
        if(SortOrder == sortOrder) 
            return;
        
        Raise(new ImageSortOrderUpdated(
            OccurredAt: now,
            ImageId: Id,
            SortOrder: sortOrder));
    }

    public void ChangeStoragePath(
        DateTimeOffset now,
        string storagePath)
    {
        EnsureNotDeleted();
        
        ProductImageRules.StoragePathValidate(storagePath);
        
        if(StoragePath == storagePath) 
            return;
        
        Raise(new ImageStoragePathUpdated(
            OccurredAt: now,
            ImageId: Id,
            StoragePath: storagePath));
    }

    public void ChangeDimension(
        DateTimeOffset now,
        int weight,
        int height)
    {
        EnsureNotDeleted();
        
        ProductImageRules.WeightAndHeightValidate(
            weight: weight, 
            height: height);
        
        if(Weight == weight && Height == height) 
            return;
        
        Raise(new ImageDimensionChanged(
            OccurredAt: now,
            ImageId: Id,
            Weight: weight,
            Height: height));
    }

    public void ChangeFileSizeBytes(
        DateTimeOffset now,
        long fileSizeBytes)
    {
        EnsureNotDeleted();
            
        Raise(new ImageFileSizeBytesChanged(
            OccurredAt: now,
            ImageId: Id,
            FileSizeBytes: fileSizeBytes));
    }
    
    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        Raise(new ImageRemoved(
            OccurredAt: now,
            VariantId: Id,
            ImageId: Id));
    }

    public void Restore(DateTimeOffset now)
    {
        if(!IsDeleted)
            throw new DomainException("Image was not deleted.");
        
        Raise(new ImageRestored(
            OccurredAt: now,
            VariantId: Id,
            ImageId: Id));
    }
    
    public void SetAsMain(DateTimeOffset now)
    {
        if(IsMain) return;

        Raise(new ImageMainSet(
            OccurredAt: now,
            ImageId: Id));
    }
    
    public void UnsetAsMain(DateTimeOffset now)
    {
        if(!IsMain) return;

        Raise(new ImageMainUnset(
            OccurredAt: now,
            ImageId: Id));
    }
    
    protected override void Apply(object @event)
    {
        switch (@event)
        {
            case ImageCreated e:
                Id = e.ImageId;
                ProductVariantId = e.VariantId;
                OriginalFileName = e.OriginalFileName;
                StoragePath = e.StoragePath;
                UploadedAt = e.OccurredAt;
                IsMain = e.IsMain;
                FileSizeBytes = e.FileSizeBytes;
                SortOrder = e.SortOrder;
                Weight = e.Weight;
                Height = e.Height;
                IsDeleted = false;
                break;
            
            case ImageSortOrderUpdated e:
                SortOrder = e.SortOrder;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ImageStoragePathUpdated e:
                StoragePath = e.StoragePath;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ImageDimensionChanged e:
                Weight = e.Weight;
                Height = e.Height;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ImageFileSizeBytesChanged e:
                FileSizeBytes = e.FileSizeBytes;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ImageMainUnset e:
                IsMain = false;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ImageMainSet e:
                IsMain = true;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ImageRemoved e:
                IsDeleted = true;
                DeletedAt = e.OccurredAt;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ImageRestored e:
                IsDeleted = false;
                DeletedAt = null;
                UpdatedAt = e.OccurredAt;
                break;
        }
    }
    
    /// <summary>
    /// Ensures the variant image is not deleted before performing operations.
    /// </summary>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="DomainException">Thrown when entity is deleted.</exception>
    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Entity is deleted.");
    }
}
    /*internal static ProductImage Reconstitute(
        Guid productVariantId,
        string originalFileName,
        string storagePath,
        long fileSizeBytes,
        bool isMain,
        short sortOrder,
        int weight, 
        int height,
        bool isDeleted,
        DateTimeOffset uploadAt,
        DateTimeOffset? updatedAt,
        DateTimeOffset? deletedAt)
    {
        return new ProductImage()
        {
            Id = Guid.NewGuid(),
            ProductVariantId = productVariantId,
            OriginalFileName = originalFileName,
            StoragePath = storagePath,
            FileSizeBytes = fileSizeBytes,
            IsMain = isMain,
            SortOrder = sortOrder,
            Weight = weight,
            Height = height,
            IsDeleted = isDeleted,
            UploadedAt = uploadAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt
        };
    }*/
