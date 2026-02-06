using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

internal static class ProductImageRules
{
    private const int MaxProductImagePathLength = 500;
    private const int MinProductImagePathLength = 25;

    private const long MaxFileSizeBytes         = 50 * 1024 * 1024; /* 50 mb */
    private const long MinFileSizeBytes         = 1;

    private const int MaxImageDimension         = 5000;
    private const int MinImageDimension         = 50;
    
    private const short MaxImageSortOrder       = 50;
    private const short MinImageSortOrder       = 1;

    internal static void CreateProductImageValidation(
        Guid imageId,
        Guid productVariantId,
        string originalFileName,
        string storagePath,
        long fileSizeBytes,
        short sortOrder,
        int weight, 
        int height)
    {
        ImageIdValidate(imageId);
        ProductVariantIdValidate(productVariantId);
        OriginalFilenameValidate(originalFileName);
        StoragePathValidate(storagePath);
        FileSizeBytesValidate(fileSizeBytes);
        SortOrderValidate(sortOrder);
        WeightAndHeightValidate(weight, height);
    }

    internal static void ImageIdValidate(Guid imageId)
    {
        if(imageId == Guid.Empty)
            throw new DomainException("Image Id  cannot be guid empty.");
    }
    
    internal static void ProductVariantIdValidate(Guid productVariantId)
    {
        if(productVariantId == Guid.Empty)
            throw new DomainException("Product variant Id  cannot be guid empty.");
    }

    internal static void OriginalFilenameValidate(string originalFileName)
    {
        if (string.IsNullOrWhiteSpace(originalFileName))
            throw new DomainException("Product image file name cannot be string empty.");
    }

    internal static void StoragePathValidate(string storagePath)
    {
        if (string.IsNullOrWhiteSpace(storagePath))
            throw new DomainException("Product image storage path cannot be string empty.");
        
        if(storagePath.Length is > MaxProductImagePathLength or < MinProductImagePathLength)
            throw new DomainException($"Product image storage path must be between {MaxProductImagePathLength} and {MinProductImagePathLength}.");
    }
    
    internal static void FileSizeBytesValidate(long fileSizeBytes)
    {
        if (fileSizeBytes is < MinFileSizeBytes or > MaxFileSizeBytes)
            throw new DomainException($"Product image File Size Bytes must be between {MinFileSizeBytes} and {MaxFileSizeBytes}.");
    }
    
    internal static void SortOrderValidate(short sortOrder)
    {
        if (sortOrder is > MaxImageSortOrder or < MinImageSortOrder)
            throw new DomainException($"Product image sort order must be between {MaxImageSortOrder} and {MinImageSortOrder}.");
    }
    
    internal static void WeightAndHeightValidate(int weight, int height)
    {
        if (weight is > MaxImageDimension or < MinImageDimension)
            throw new DomainException($"Product image weight must be between {MaxImageDimension} and {MinImageDimension}.");
        
        if (height is > MaxImageDimension or < MinImageDimension)
            throw new DomainException($"Product image height order must be between {MaxImageDimension} and {MinImageDimension}.");
    }
}