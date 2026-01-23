using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a product image physical entity with lifecycle and invariants.
/// </summary>
public class ProductImageEntity
{
    private readonly ProductVariant? _productVariant;
    
    public Guid Id { get; private set; }
    public string OriginalFileName { get; private set; } = string.Empty;
    public string StoragePath { get; private set; } = string.Empty;
    public long FileSizeBytes { get; private set; }
    public bool IsMain { get; private set; }
    public short SortOrder { get; private set; } 
    public DateTimeOffset UploadedAt { get; private set; } 
    public int Weight { get; private set; }
    public int Height { get; private set; }
    public Guid ProductVariantId { get; private set; }
    
    public bool IsDeleted { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private const int MaxProductImagePathLength = 500;
    private const int MinProductImagePathLength = 25;

    private const long MaxFileSizeBytes         = 50 * 1024 * 1024; /* 50 mb */
    private const long MinFileSizeBytes         = 1;

    private const int MaxImageDimension         = 5000;
    private const int MinImageDimension         = 50;
    
    private const short MaxImageSortOrder       = 50;
    private const short MinImageSortOrder       = 1;
    
    private ProductImageEntity() { }

    // TODO: move the validation in a separate methods
    public static ProductImageEntity Create(
        DateTimeOffset now,
        Guid productVariantId,
        string originalFileName,
        string storagePath,
        long fileSizeBytes,
        bool isMain,
        short sortOrder,
        int weight, 
        int height)
    {
        if(productVariantId == Guid.Empty)
            throw new DomainException("Product variant Id  cannot be guid empty.");
        
        if (string.IsNullOrWhiteSpace(originalFileName))
            throw new DomainException("Product image file name cannot be string empty.");
        
        if (string.IsNullOrWhiteSpace(storagePath))
            throw new DomainException("Product image storage path cannot be string empty.");
        
        if(storagePath.Length is > MaxProductImagePathLength or < MinProductImagePathLength)
            throw new DomainException($"Product image storage path must be between {MaxProductImagePathLength} and {MinProductImagePathLength}.");
        
        if (fileSizeBytes is < MinFileSizeBytes or > MaxFileSizeBytes)
            throw new DomainException($"Product image File Size Bytes must be between {MinFileSizeBytes} and {MaxFileSizeBytes}.");
        
        if (sortOrder is > MaxImageSortOrder or < MinImageSortOrder)
            throw new DomainException($"Product image sort order must be between {MaxImageSortOrder} and {MinImageSortOrder}.");
        
        if (weight is > MaxImageDimension or < MinImageDimension)
            throw new DomainException($"Product image weight must be between {MaxImageDimension} and {MinImageDimension}.");
        
        if (height is > MaxImageDimension or < MinImageDimension)
            throw new DomainException($"Product image height order must be between {MaxImageDimension} and {MinImageDimension}.");
        
        var image = new ProductImageEntity()
        {
            ProductVariantId = productVariantId,
            OriginalFileName = originalFileName,
            StoragePath = storagePath,
            UploadedAt = now,
            IsMain = isMain,
            FileSizeBytes = fileSizeBytes,
            SortOrder = sortOrder,
            Weight = weight,
            Height = height,
            IsDeleted = false,
        };

        return image;
    }

    public void SetAsMain(DateTimeOffset now)
    {
        EnsureNotDeleted();

        IsMain = true;
        UpdatedAt = now;
    }
    
    public void UnsetAsMain(DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if(!IsMain) return;

        IsMain = false;
        UpdatedAt = now;
    }

    public void ChangeSortOrder(
        DateTimeOffset now,
        short sortOrder)
    {
        EnsureNotDeleted();
        
        if(SortOrder == sortOrder) return;

        SortOrder = sortOrder;
        UpdatedAt = now;
    }

    public void ChangeStoragePath(
        DateTimeOffset now,
        string storagePath)
    {
        EnsureNotDeleted();
        
        if (string.IsNullOrWhiteSpace(storagePath))
            throw new DomainException("Product image storage path cannot be string empty.");
        
        if(storagePath.Length is > MaxProductImagePathLength or < MinProductImagePathLength)
            throw new DomainException($"Product image storage path must be between {MaxProductImagePathLength} and {MinProductImagePathLength}.");
        
        if(StoragePath == storagePath) return;

        StoragePath = storagePath;
        UpdatedAt = now;
    }

    public void ChangeDimension(
        DateTimeOffset now,
        int weight,
        int height)
    {
        EnsureNotDeleted();
        
        if (weight is > MaxImageDimension or < MinImageDimension)
            throw new DomainException($"Product image weight must be between {MaxImageDimension} and {MinImageDimension}.");
        
        if (height is > MaxImageDimension or < MinImageDimension)
            throw new DomainException($"Product image height order must be between {MaxImageDimension} and {MinImageDimension}.");

        if(Weight == weight && Height == height) 
            return;
        
        Weight = weight;
        Height = height;

        UpdatedAt = now;
    }

    public void ChangeFileSizeBytes(
        DateTimeOffset now,
        long fileSizeBytes)
    {
        EnsureNotDeleted();
        
        if (fileSizeBytes is < MinFileSizeBytes or > MaxFileSizeBytes)
            throw new DomainException($"Product image File Size Bytes must be between {MinFileSizeBytes} and {MaxFileSizeBytes}.");

        if(FileSizeBytes == fileSizeBytes)
            return;
        
        FileSizeBytes = fileSizeBytes;
    }

    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted();

        IsDeleted = true;
        
        DeletedAt = now;
        UpdatedAt = now;
    }
    
    public void Restore(DateTimeOffset now)
    {
        if (!IsDeleted)
            throw new DomainException("Product Image is not deleted!");
        
        IsDeleted = false;
        
        DeletedAt = null;
        UpdatedAt = now;
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new DomainException("Product Image already was deleted!");
    }
    
}