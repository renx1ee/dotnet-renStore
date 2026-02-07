using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

internal static class ProductVariantRules
{
    private const int MaxProductNameLength = 500;
    private const int MinProductNameLength = 25;
    
    private const int MaxImagesCount       = 50;
    private const int MaxAttributesCount   = 50;
    
    internal static void ValidateProductId(Guid productId)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product Id cannot be guid empty.");
    }
    
    internal static void ValidateColorId(int colorId)
    {
        if (colorId <= 0)
            throw new DomainException("Color Id cannot be less then 1.");
    }
    
    internal static void ValidateName(string name)
    {
        if(name.Length is < MinProductNameLength or > MaxProductNameLength)
            throw new DomainException($"Product name must be between {MinProductNameLength} and {MaxProductNameLength}.");
    }
    
    internal static void ValidateUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new DomainException("Url cannot be string empty.");
    }
    
    internal static void ValidateInStock(int inStock)
    {
        if(inStock < 0)
            throw new DomainException("InStock cannot be less then 0.");
    }

    internal static void MaxAttributesCountValidation(int count)
    {
        if (count >= MaxAttributesCount)
            throw new DomainException($"Attributes count must be less then {MaxAttributesCount}.");
    }
    
    internal static void MaxImagesCountValidation(int count)
    {
        if (count >= MaxImagesCount)
            throw new DomainException($"Product images count must be less then {MaxImagesCount}.");
    }
}