using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Rules;

/// <summary>
/// Business rules and validation logic specific to product variants.
/// Enforces constraints on variant creation, modification, and relationships.
/// </summary>
internal static class ProductVariantRules
{
    private const int MaxProductNameLength = 500;
    private const int MinProductNameLength = 10;
    
    private const int MaxUrlLength = 500;
    private const int MinUrlLength = 25;
    
    /// <summary>
    /// Validates a product identifier for variant association.
    /// </summary>
    /// <param name="productId">Product identifier to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when identifier is <see cref="Guid.Empty"/>
    /// </exception>
    /// <remarks>
    /// Ensures variants are always associated with a valid parent product.
    /// </remarks>
    internal static void ValidateProductId(Guid productId)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product Id cannot be guid empty.");
    }
    
    /// <summary>
    /// Validates a color identifier for variant configuration.
    /// </summary>
    /// <param name="colorId">Color identifier to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when identifier is zero or negative
    /// </exception>
    /// <remarks>
    /// Color is a required variant attribute for product differentiation.
    /// </remarks>
    internal static void ValidateColorId(int colorId)
    {
        if (colorId <= 0)
            throw new DomainException("Color Id cannot be less then 1.");
    }
    
    internal static void ValidateArticle(long article)
    {
        if (article <= 0)
            throw new DomainException("Article cannot be less then 1.");
    }
    
    internal static string ValidateAndTrimName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be null or white space");
        
        var trimmedName = name.Trim();
        
        if(trimmedName.Length is < MinProductNameLength or > MaxProductNameLength)
            throw new DomainException($"Product name must be between {MinProductNameLength} and {MaxProductNameLength}.");

        return trimmedName;
    }
    
    /// <summary>
    /// Validates a product variant URL slug.
    /// </summary>
    /// <param name="url">URL slug to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when URL is null, empty, or whitespace
    /// </exception>
    /// <remarks>
    /// URL slugs are used for SEO-friendly product links and must be stable over time.
    /// </remarks>
    internal static string ValidateAndTrimUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new DomainException("Url cannot be string empty.");

        var trimmedUrl = url.Trim();
        
        if(trimmedUrl.Length is < MinUrlLength or > MaxUrlLength)
            throw new DomainException($"Product url must be between {MinUrlLength} and {MaxUrlLength}.");

        return trimmedUrl;
    }
}