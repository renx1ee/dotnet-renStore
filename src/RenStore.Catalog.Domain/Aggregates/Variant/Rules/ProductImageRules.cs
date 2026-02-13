using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Rules;

/// <summary>
/// Business rules and validation logic for product images.
/// Ensures image quality, performance, and storage requirements are met.
/// </summary>
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
    
    /// <summary>
    /// Comprehensive validation for all product image creation parameters.
    /// </summary>
    /// <param name="imageId">Unique identifier for the image</param>
    /// <param name="productVariantId">Parent variant identifier</param>
    /// <param name="originalFileName">Original uploaded file name</param>
    /// <param name="storagePath">Path to stored image file</param>
    /// <param name="fileSizeBytes">Size of image file in bytes</param>
    /// <param name="sortOrder">Display sequence in gallery</param>
    /// <param name="weight">Image width in pixels</param>
    /// <param name="height">Image height in pixels</param>
    /// <exception cref="DomainException">
    /// Thrown when any validation rule is violated
    /// </exception>
    /// <remarks>
    /// Performs all necessary validations in a single call for consistency.
    /// Used during image upload and processing workflows.
    /// </remarks>
    internal static void CreateProductImageValidation(
        Guid productVariantId,
        string originalFileName,
        string storagePath,
        long fileSizeBytes,
        short sortOrder,
        int weight, 
        int height)
    {
        ProductVariantIdValidate(productVariantId);
        OriginalFilenameValidate(originalFileName);
        StoragePathValidate(storagePath);
        FileSizeBytesValidate(fileSizeBytes);
        SortOrderValidate(sortOrder);
        WeightAndHeightValidate(weight, height);
    }

    /// <summary>
    /// Validates an image identifier.
    /// </summary>
    /// <param name="imageId">Image identifier to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when identifier is <see cref="Guid.Empty"/>
    /// </exception>
    /// <remarks>
    /// Ensures each image has a unique, valid identifier for tracking.
    /// </remarks>
    internal static void ImageIdValidate(Guid imageId)
    {
        if(imageId == Guid.Empty)
            throw new DomainException("Image Id  cannot be guid empty.");
    }
    
    /// <summary>
    /// Validates a product variant identifier.
    /// </summary>
    /// <param name="productVariantId">Variant identifier to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when identifier is <see cref="Guid.Empty"/>
    /// </exception>
    /// <remarks>
    /// Ensures images are properly associated with a parent product variant.
    /// </remarks>
    internal static void ProductVariantIdValidate(Guid productVariantId)
    {
        if(productVariantId == Guid.Empty)
            throw new DomainException("Product variant Id  cannot be guid empty.");
    }

    /// <summary>
    /// Validates the original file name of an uploaded image.
    /// </summary>
    /// <param name="originalFileName">File name to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when file name is null, empty, or whitespace
    /// </exception>
    /// <remarks>
    /// Preserves original upload metadata for reference and troubleshooting.
    /// </remarks>
    internal static void OriginalFilenameValidate(string originalFileName)
    {
        if (string.IsNullOrWhiteSpace(originalFileName))
            throw new DomainException("Product image file name cannot be string empty.");
    }

    /// <summary>
    /// Validates the storage path for an image file.
    /// </summary>
    /// <param name="storagePath">Storage path to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Path is null, empty, or whitespace
    /// - Path length is outside allowed constraints
    /// </exception>
    /// <remarks>
    /// Storage paths must be valid and within reasonable length limits for filesystem compatibility.
    /// </remarks>
    internal static void StoragePathValidate(string storagePath)
    {
        if (string.IsNullOrWhiteSpace(storagePath))
            throw new DomainException("Product image storage path cannot be string empty.");
        
        if(storagePath.Length is > MaxProductImagePathLength or < MinProductImagePathLength)
            throw new DomainException($"Product image storage path must be between {MaxProductImagePathLength} and {MinProductImagePathLength}.");
    }
    
    /// <summary>
    /// Validates image file size.
    /// </summary>
    /// <param name="fileSizeBytes">File size in bytes to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when file size is outside allowed constraints (1 byte to 50 MB)
    /// </exception>
    /// <remarks>
    /// Prevents excessively large files that could impact performance and storage costs.
    /// </remarks>
    internal static void FileSizeBytesValidate(long fileSizeBytes)
    {
        if (fileSizeBytes is < MinFileSizeBytes or > MaxFileSizeBytes)
            throw new DomainException($"Product image File Size Bytes must be between {MinFileSizeBytes} and {MaxFileSizeBytes}.");
    }
    
    /// <summary>
    /// Validates image display sort order.
    /// </summary>
    /// <param name="sortOrder">Sort order to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when sort order is outside allowed range (1-50)
    /// </exception>
    /// <remarks>
    /// Determines display sequence in product image galleries.
    /// Lower numbers appear first. Maximum of 50 images per variant.
    /// </remarks>
    internal static void SortOrderValidate(short sortOrder)
    {
        if (sortOrder is > MaxImageSortOrder or < MinImageSortOrder)
            throw new DomainException($"Product image sort order must be between {MaxImageSortOrder} and {MinImageSortOrder}.");
    }
    
    /// <summary>
    /// Validates image dimensions (width and height).
    /// </summary>
    /// <param name="weight">Image width in pixels</param>
    /// <param name="height">Image height in pixels</param>
    /// <exception cref="DomainException">
    /// Thrown when either dimension is outside allowed range (50-5000 pixels)
    /// </exception>
    /// <remarks>
    /// Ensures images are within reasonable size limits for display optimization.
    /// Prevents minimal or excessively large images.
    /// </remarks>
    internal static void WeightAndHeightValidate(int weight, int height)
    {
        if (weight is > MaxImageDimension or < MinImageDimension)
            throw new DomainException($"Product image weight must be between {MaxImageDimension} and {MinImageDimension}.");
        
        if (height is > MaxImageDimension or < MinImageDimension)
            throw new DomainException($"Product image height order must be between {MaxImageDimension} and {MinImageDimension}.");
    }
}