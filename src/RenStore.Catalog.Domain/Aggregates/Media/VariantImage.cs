using RenStore.Catalog.Domain.Aggregates.Media.Events;
using RenStore.Catalog.Domain.Aggregates.Media.Rules;
using RenStore.Catalog.Domain.Aggregates.Variant.Rules;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Media;

/// <summary>
/// Represents a product image physical entity with lifecycle and invariants.
/// </summary>
public sealed class VariantImage
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    public Guid Id { get; private set; }
    public string OriginalFileName { get; private set; }
    public string StoragePath { get; private set; }
    public long FileSizeBytes { get; private set; }
    public bool IsMain { get; private set; }
    public int SortOrder { get; private set; } 
    public int Weight { get; private set; }
    public int Height { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset UploadedAt { get; private set; } 
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public Guid UpdatedById { get; private set; } 
    public string UpdatedByRole { get; private set; } 
    public Guid VariantId { get; private set; }
    
    private VariantImage() { }
    
    public static VariantImage Create(
        DateTimeOffset now,
        Guid variantId,
        string originalFileName,
        string storagePath,
        long fileSizeBytes,
        bool isMain,
        int sortOrder,
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
        var image = new VariantImage();
        
        image.Raise(new ImageCreatedEvent(
            EventId: Guid.NewGuid(), 
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
        int sortOrder)
    {
        EnsureNotDeleted();
        
        if(SortOrder == sortOrder) 
            return;
        
        Raise(new ImageSortOrderUpdatedEvent(
            EventId: Guid.NewGuid(), 
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
        
        Raise(new ImageStoragePathUpdatedEvent(
            EventId: Guid.NewGuid(), 
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
        
        Raise(new ImageDimensionUpdatedEvent(
            EventId: Guid.NewGuid(), 
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
        
        ProductImageRules.FileSizeBytesValidate(fileSizeBytes);

        if (FileSizeBytes == fileSizeBytes) return;
            
        Raise(new ImageFileSizeBytesUpdatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            ImageId: Id,
            FileSizeBytes: fileSizeBytes));
    }
    
    public void SetAsMain(DateTimeOffset now)
    {
        if(IsMain) return;

        Raise(new ImageMainSetEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            ImageId: Id));
    }
    
    public void UnsetAsMain(DateTimeOffset now)
    {
        if(!IsMain) return;

        Raise(new ImageMainUnsetEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            VariantId: VariantId,
            ImageId: Id));
    }
    
    public void Delete(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        VariantImageRules.UpdatedByParametersValidation(
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        Raise(new VariantImageRemovedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            ImageId: Id));
    }

    public void Restore(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        if(!IsDeleted)
            throw new DomainException("Image was not deleted.");
        
        VariantImageRules.UpdatedByParametersValidation(
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        Raise(new ImageRestoredEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            ImageId: Id));
    }
    
    protected override void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case ImageCreatedEvent e:
                Id = e.ImageId;
                VariantId = e.VariantId;
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
            
            case ImageSortOrderUpdatedEvent e:
                SortOrder = e.SortOrder;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ImageStoragePathUpdatedEvent e:
                StoragePath = e.StoragePath;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ImageDimensionUpdatedEvent e:
                Weight = e.Weight;
                Height = e.Height;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ImageFileSizeBytesUpdatedEvent e:
                FileSizeBytes = e.FileSizeBytes;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ImageMainUnsetEvent e:
                IsMain = false;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ImageMainSetEvent e:
                IsMain = true;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageRemovedEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                IsDeleted = true;
                DeletedAt = e.OccurredAt;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ImageRestoredEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                IsDeleted = false;
                DeletedAt = null;
                UpdatedAt = e.OccurredAt;
                break;
        }
    }
    
    public static VariantImage Rehydrate(IEnumerable<IDomainEvent> history)
    {
        var variantImage = new VariantImage();
        
        foreach (var @event in history)
        {
            variantImage.Apply(@event);
            variantImage.Version++;
        }

        return variantImage;
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
