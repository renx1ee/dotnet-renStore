using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

/// <summary>
/// Represents a product image physical entity with lifecycle and invariants.
/// </summary>
public class ProductImage
{
    private readonly ProductVariant? _productVariant;
    
    public Guid Id { get; private set; }
    public string OriginalFileName { get; private set; } = string.Empty;
    public string StoragePath { get; private set; } = string.Empty;
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
        Guid imageId,
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
        return new ProductImage()
        {
            Id = imageId,
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
    }
    
    public static ProductImage Reconstitute(
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
    }
    
    public void SetAsMain(DateTimeOffset now)
    {
        if(IsMain) return;

        IsMain = true;
        UpdatedAt = now;
    }
    
    public void UnsetAsMain(DateTimeOffset now)
    {
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

        ProductImageRules.StoragePathValidate(storagePath);
        
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

        ProductImageRules.WeightAndHeightValidate(
            weight: weight, 
            height: height);

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
        
        ProductImageRules.FileSizeBytesValidate(fileSizeBytes);

        if(FileSizeBytes == fileSizeBytes)
            return;
        
        FileSizeBytes = fileSizeBytes;
        UpdatedAt = now;
    }
    
    internal void Delete(DateTimeOffset now)
    {
        IsDeleted = true;
        DeletedAt = now;
        UpdatedAt = now;
    }
    
    internal void Restore(DateTimeOffset now)
    {
        IsDeleted = true;
        UpdatedAt = now;
        DeletedAt = null;
    }
    
    /// <summary>
    /// Ensures the attribute is not deleted before performing operations.
    /// </summary>
    /// <param name="message">Optional custom error message</param>
    /// <exception cref="DomainException">Thrown when attribute is deleted</exception>
    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Entity is deleted.");
    }
}