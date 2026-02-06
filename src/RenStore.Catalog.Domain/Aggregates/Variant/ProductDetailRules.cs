using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

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
    
    internal static void CountryOfManufactureValidate(int countryOfManufactureId)
    {
        if(countryOfManufactureId <= 0)
            throw new DomainException("Product Detail country of manufacture ID must be more then 0.");
    }
    
    internal static void ProductVariantIdValidate(Guid productVariantId)
    {
        if(productVariantId == Guid.Empty)
            throw new DomainException("Product Detail product variant id cannot be guid empty.");
    }
    
    internal static string DescriptionNormalizedAndValidate(string description)
    {
        if(string.IsNullOrWhiteSpace(description))
            throw new DomainException("Product Detail Description cannot be null or whitespace.");
        
        var trimmedDescription = description.Trim();
        
        if(trimmedDescription.Length is > MaxDescriptionLength or < MinDescriptionLength)
            throw new DomainException($"Product Detail Description length must between {MaxDescriptionLength} and {MinDescriptionLength}.");

        return trimmedDescription;
    }

    internal static string ModelFeaturesNormalizedAndValidate(string? modelFeatures)
    {
        if (string.IsNullOrWhiteSpace(modelFeatures)) 
            return string.Empty;
        
        var trimmedModelFeatures = modelFeatures.Trim();
        
        if(trimmedModelFeatures.Length is > MaxModelFeaturesLength or < MinModelFeaturesLength)
            throw new DomainException($"Product Detail model features length must between {MaxModelFeaturesLength} and {MinModelFeaturesLength}.");

        return trimmedModelFeatures;
    }
    
    internal static string DecorativeElementsNormalizedAndValidate(string? decorativeElements)
    {
        if(string.IsNullOrWhiteSpace(decorativeElements))
            return string.Empty;
        
        var trimmedDecorativeElements = decorativeElements.Trim();
        
        if(trimmedDecorativeElements.Length is > MaxDecorativeElementsLength or < MinDecorativeElementsLength)
            throw new DomainException($"Product Detail decorative elements length must between {MaxDecorativeElementsLength} and {MinDecorativeElementsLength}.");

        return trimmedDecorativeElements;
    }
    
    internal static string EquipmentNormalizedAndValidate(string? equipment)
    {
        if(string.IsNullOrWhiteSpace(equipment))
            return string.Empty;
        
        var trimmedEquipment = equipment.Trim();
        
        if(trimmedEquipment.Length is > MaxEquipmentLength or < MinEquipmentLength)
            throw new DomainException($"Product Detail equipment length must between {MaxEquipmentLength} and {MinEquipmentLength}.");

        return trimmedEquipment;
    }
    
    internal static string CompositionNormalizedAndValidate(string? composition)
    {
        if(string.IsNullOrWhiteSpace(composition))
            return string.Empty;
        
        var trimmedComposition = composition.Trim();
        
        if(trimmedComposition.Length is > MaxCompositionLength or < MinCompositionLength)
            throw new DomainException($"Product Detail composition length must between {MaxCompositionLength} and {MinCompositionLength}.");

        return trimmedComposition;
    }
    
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