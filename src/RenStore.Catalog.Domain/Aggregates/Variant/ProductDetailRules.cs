using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

/// <summary>
/// Business rules and validation logic for product detail information.
/// Ensures comprehensive and consistent product specifications across the catalog.
/// </summary>
internal static class ProductDetailRules
{
    private const int MaxDescriptionLength        = 500;
    private const int MinDescriptionLength        = 25;
    
    private const int MaxModelFeaturesLength      = 500;
    private const int MinModelFeaturesLength      = 25;
    
    private const int MaxDecorativeElementsLength = 500;
    private const int MinDecorativeElementsLength = 25;
    
    private const int MaxEquipmentLength          = 500;
    private const int MinEquipmentLength          = 25;
    
    private const int MaxCompositionLength        = 500;
    private const int MinCompositionLength        = 25;
    
    private const int MaxCaringOfThingsLength     = 500;
    private const int MinCaringOfThingsLength     = 25;
    
    /// <summary>
    /// Validates the country of manufacture identifier.
    /// </summary>
    /// <param name="countryOfManufactureId">Country identifier to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when identifier is zero or negative
    /// </exception>
    /// <remarks>
    /// Used for regulatory compliance and country-of-origin labeling requirements.
    /// </remarks>
    internal static void CountryOfManufactureValidate(int countryOfManufactureId)
    {
        if(countryOfManufactureId <= 0)
            throw new DomainException("Product Detail country of manufacture ID must be more then 0.");
    }
    
    /// <summary>
    /// Validates the product variant identifier.
    /// </summary>
    /// <param name="productVariantId">Variant identifier to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when identifier is <see cref="Guid.Empty"/>
    /// </exception>
    /// <remarks>
    /// Ensures details are always linked to a valid parent variant.
    /// </remarks>
    internal static void ProductVariantIdValidate(Guid productVariantId)
    {
        if(productVariantId == Guid.Empty)
            throw new DomainException("Product Detail product variant id cannot be guid empty.");
    }
    
    /// <summary>
    /// Normalizes and validates the product description.
    /// </summary>
    /// <param name="description">Product description to validate</param>
    /// <returns>Trimmed and validated description</returns>
    /// <exception cref="DomainException">
    /// Thrown when description is null, empty, or outside length constraints
    /// </exception>
    /// <remarks>
    /// Descriptions provide comprehensive product information for customer decision-making.
    /// Required field for all product variants.
    /// </remarks>
    internal static string DescriptionNormalizedAndValidate(string description)
    {
        if(string.IsNullOrWhiteSpace(description))
            throw new DomainException("Product Detail Description cannot be null or whitespace.");
        
        var trimmedDescription = description.Trim();
        
        if(trimmedDescription.Length is > MaxDescriptionLength or < MinDescriptionLength)
            throw new DomainException($"Product Detail Description length must between {MaxDescriptionLength} and {MinDescriptionLength}.");

        return trimmedDescription;
    }

    /// <summary>
    /// Normalizes and validates model features and specifications.
    /// </summary>
    /// <param name="modelFeatures">Model features to validate (optional)</param>
    /// <returns>Trimmed model features or empty string</returns>
    /// <exception cref="DomainException">
    /// Thrown when provided value is outside length constraints
    /// </exception>
    /// <remarks>
    /// Describes technical specifications and unique features of the product model.
    /// Optional field; returns empty string if null or whitespace.
    /// </remarks>
    internal static string ModelFeaturesNormalizedAndValidate(string? modelFeatures)
    {
        if (string.IsNullOrWhiteSpace(modelFeatures)) 
            return string.Empty;
        
        var trimmedModelFeatures = modelFeatures.Trim();
        
        if(trimmedModelFeatures.Length is > MaxModelFeaturesLength or < MinModelFeaturesLength)
            throw new DomainException($"Product Detail model features length must between {MaxModelFeaturesLength} and {MinModelFeaturesLength}.");

        return trimmedModelFeatures;
    }
    
    /// <summary>
    /// Normalizes and validates decorative elements description.
    /// </summary>
    /// <param name="decorativeElements">Decorative elements to validate (optional)</param>
    /// <returns>Trimmed decorative elements or empty string</returns>
    /// <exception cref="DomainException">
    /// Thrown when provided value is outside length constraints
    /// </exception>
    /// <remarks>
    /// Describes aesthetic and design elements of the product.
    /// Optional field; particularly relevant for fashion and home decor items.
    /// </remarks>
    internal static string DecorativeElementsNormalizedAndValidate(string? decorativeElements)
    {
        if(string.IsNullOrWhiteSpace(decorativeElements))
            return string.Empty;
        
        var trimmedDecorativeElements = decorativeElements.Trim();
        
        if(trimmedDecorativeElements.Length is > MaxDecorativeElementsLength or < MinDecorativeElementsLength)
            throw new DomainException($"Product Detail decorative elements length must between {MaxDecorativeElementsLength} and {MinDecorativeElementsLength}.");

        return trimmedDecorativeElements;
    }
    
    /// <summary>
    /// Normalizes and validates included equipment and accessories.
    /// </summary>
    /// <param name="equipment">Equipment list to validate (optional)</param>
    /// <returns>Trimmed equipment list or empty string</returns>
    /// <exception cref="DomainException">
    /// Thrown when provided value is outside length constraints
    /// </exception>
    /// <remarks>
    /// Lists items included with the product (batteries, cables, manuals, etc.).
    /// Important for setting customer expectations and avoiding returns.
    /// </remarks>
    internal static string EquipmentNormalizedAndValidate(string? equipment)
    {
        if(string.IsNullOrWhiteSpace(equipment))
            return string.Empty;
        
        var trimmedEquipment = equipment.Trim();
        
        if(trimmedEquipment.Length is > MaxEquipmentLength or < MinEquipmentLength)
            throw new DomainException($"Product Detail equipment length must between {MaxEquipmentLength} and {MinEquipmentLength}.");

        return trimmedEquipment;
    }
    
    /// <summary>
    /// Normalizes and validates material composition.
    /// </summary>
    /// <param name="composition">Material composition to validate (optional)</param>
    /// <returns>Trimmed composition or empty string</returns>
    /// <exception cref="DomainException">
    /// Thrown when provided value is outside length constraints
    /// </exception>
    /// <remarks>
    /// Specifies materials and their percentages (e.g., "100% Cotton", "Leather 80%, Polyester 20%").
    /// Required for apparel, textiles, and regulated products.
    /// </remarks>
    internal static string CompositionNormalizedAndValidate(string? composition)
    {
        if(string.IsNullOrWhiteSpace(composition))
            return string.Empty;
        
        var trimmedComposition = composition.Trim();
        
        if(trimmedComposition.Length is > MaxCompositionLength or < MinCompositionLength)
            throw new DomainException($"Product Detail composition length must between {MaxCompositionLength} and {MinCompositionLength}.");

        return trimmedComposition;
    }
    
    /// <summary>
    /// Normalizes and validates care and maintenance instructions.
    /// </summary>
    /// <param name="caringOfThings">Care instructions to validate (optional)</param>
    /// <returns>Trimmed care instructions or empty string</returns>
    /// <exception cref="DomainException">
    /// Thrown when provided value is outside length constraints
    /// </exception>
    /// <remarks>
    /// Provides washing, cleaning, and maintenance guidelines.
    /// Important for product longevity and customer satisfaction.
    /// </remarks>
    internal static string CaringOfThingsNormalizedAndValidate(string? caringOfThings)
    {
        if(string.IsNullOrWhiteSpace(caringOfThings))
            return string.Empty;
        
        var trimmedCaringOfThings = caringOfThings.Trim();
        
        if(trimmedCaringOfThings.Length is > MaxCaringOfThingsLength or < MinCaringOfThingsLength)
            throw new DomainException($"Product Detail caring of things length must between {MaxCaringOfThingsLength} and {MinCaringOfThingsLength}.");

        return trimmedCaringOfThings;
    }
}