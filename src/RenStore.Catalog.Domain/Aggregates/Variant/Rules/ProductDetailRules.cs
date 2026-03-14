using RenStore.Catalog.Domain.Constants;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Rules;

/// <summary>
/// Business rules and validation logic for product detail information.
/// Ensures comprehensive and consistent product specifications across the catalog.
/// </summary>
internal static class ProductDetailRules
{
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
        
        if(trimmedDescription.Length is > CatalogConstants.ProductDetail.MaxDescriptionLength or < CatalogConstants.ProductDetail.MinDescriptionLength)
            throw new DomainException($"Product Detail Description length must between {CatalogConstants.ProductDetail.MaxDescriptionLength} and {CatalogConstants.ProductDetail.MinDescriptionLength}.");

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
    internal static string? ModelFeaturesNormalizedAndValidate(string? modelFeatures)
    {
        if (string.IsNullOrWhiteSpace(modelFeatures)) 
            return null;
        
        var trimmedModelFeatures = modelFeatures.Trim();
        
        if(trimmedModelFeatures.Length is > CatalogConstants.ProductDetail.MaxModelFeaturesLength or < CatalogConstants.ProductDetail.MinModelFeaturesLength)
            throw new DomainException($"Product Detail model features length must between {CatalogConstants.ProductDetail.MaxModelFeaturesLength} and {CatalogConstants.ProductDetail.MinModelFeaturesLength}.");

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
    internal static string? DecorativeElementsNormalizedAndValidate(string? decorativeElements)
    {
        if(string.IsNullOrWhiteSpace(decorativeElements))
            return null;
        
        var trimmedDecorativeElements = decorativeElements.Trim();
        
        if(trimmedDecorativeElements.Length is > CatalogConstants.ProductDetail.MaxDecorativeElementsLength or < CatalogConstants.ProductDetail.MinDecorativeElementsLength)
            throw new DomainException(
                $"Product Detail decorative elements length must between {CatalogConstants.ProductDetail.MaxDecorativeElementsLength} and {CatalogConstants.ProductDetail.MinDecorativeElementsLength}.");

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
    internal static string? EquipmentNormalizedAndValidate(string? equipment)
    {
        if(string.IsNullOrWhiteSpace(equipment))
            return null;
        
        var trimmedEquipment = equipment.Trim();
        
        if(trimmedEquipment.Length is > CatalogConstants.ProductDetail.MaxEquipmentLength or < CatalogConstants.ProductDetail.MinEquipmentLength)
            throw new DomainException($"Product Detail equipment length must between {CatalogConstants.ProductDetail.MaxEquipmentLength} and {CatalogConstants.ProductDetail.MinEquipmentLength}.");

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
    internal static string CompositionNormalizedAndValidate(string composition)
    {
        if(string.IsNullOrWhiteSpace(composition))
            return string.Empty;
        
        var trimmedComposition = composition.Trim();
        
        if(trimmedComposition.Length is > CatalogConstants.ProductDetail.MaxCompositionLength or < CatalogConstants.ProductDetail.MinCompositionLength)
            throw new DomainException($"Product Detail composition length must between {CatalogConstants.ProductDetail.MaxCompositionLength} and {CatalogConstants.ProductDetail.MinCompositionLength}.");

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
    internal static string? CaringOfThingsNormalizedAndValidate(string? caringOfThings)
    {
        if(string.IsNullOrWhiteSpace(caringOfThings))
            return null;
        
        var trimmedCaringOfThings = caringOfThings.Trim();
        
        if(trimmedCaringOfThings.Length is > CatalogConstants.ProductDetail.MaxCaringOfThingsLength or < CatalogConstants.ProductDetail.MinCaringOfThingsLength)
            throw new DomainException($"Product Detail caring of things length must between {CatalogConstants.ProductDetail.MaxCaringOfThingsLength} and {CatalogConstants.ProductDetail.MinCaringOfThingsLength}.");

        return trimmedCaringOfThings;
    }
}