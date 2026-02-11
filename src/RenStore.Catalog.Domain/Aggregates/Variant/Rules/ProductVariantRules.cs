using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Rules;

/// <summary>
/// Business rules and validation logic specific to product variants.
/// Enforces constraints on variant creation, modification, and relationships.
/// </summary>
internal static class ProductVariantRules
{
    private const int MaxProductNameLength = 500;
    private const int MinProductNameLength = 25;
    
    private const int MaxImagesCount       = 50;
    private const int MaxAttributesCount   = 50;
    
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
    
    /// <summary>
    /// Validates a product variant display name.
    /// </summary>
    /// <param name="name">Variant name to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when name length is outside allowed constraints (25-500 characters)
    /// </exception>
    /// <remarks>
    /// Names should be descriptive enough for customer identification while maintaining reasonable length.
    /// </remarks>
    internal static void ValidateName(string name)
    {
        if(name.Length is < MinProductNameLength or > MaxProductNameLength)
            throw new DomainException($"Product name must be between {MinProductNameLength} and {MaxProductNameLength}.");
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
    internal static void ValidateUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new DomainException("Url cannot be string empty.");
    }

    /// <summary>
    /// Validates that the variant has not exceeded the maximum allowed attributes.
    /// </summary>
    /// <param name="count">Current number of attributes</param>
    /// <exception cref="DomainException">
    /// Thrown when count reaches or exceeds <see cref="MaxAttributesCount"/>
    /// </exception>
    /// <remarks>
    /// Limits complexity and maintains performance for product filtering and display.
    /// </remarks>
    internal static void MaxAttributesCountValidation(int count)
    {
        if (count >= MaxAttributesCount)
            throw new DomainException($"Attributes count must be less then {MaxAttributesCount}.");
    }
    
    /// <summary>
    /// Validates that the variant has not exceeded the maximum allowed images.
    /// </summary>
    /// <param name="count">Current number of images</param>
    /// <exception cref="DomainException">
    /// Thrown when count reaches or exceeds <see cref="MaxImagesCount"/>
    /// </exception>
    /// <remarks>
    /// Balances visual coverage with performance considerations for image galleries.
    /// </remarks>
    internal static void MaxImagesCountValidation(int count)
    {
        if (count >= MaxImagesCount)
            throw new DomainException($"Product images count must be less then {MaxImagesCount}.");
    }
}