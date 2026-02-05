using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Entities;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

/// <summary>
/// Represents a product Detail physical entity with lifecycle and invariants.
/// </summary>
public class ProductDetail
    : EntityWithSoftDeleteBase
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string ModelFeatures { get; private set; } = string.Empty;
    public string DecorativeElements { get; private set; } = string.Empty;
    public string Equipment { get; private set; } = string.Empty;
    public string Composition { get; private set; } = string.Empty;
    public string CaringOfThings { get; private set; } = string.Empty;
    public TypeOfPackaging? TypeOfPacking { get; private set; }
    public int CountryOfManufactureId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public DateTimeOffset? UpdatedAt { get; protected set; }
    public DateTimeOffset? DeletedAt { get; protected set; }

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
    
    private ProductDetail() { }

    internal static ProductDetail Create(
        DateTimeOffset now,
        int countryOfManufactureId,
        Guid productVariantId,
        string description,
        string? modelFeatures = null,
        string? decorativeElements = null,
        string? equipment = null,
        string? composition = null,
        string? caringOfThings = null,
        TypeOfPackaging? typeOfPackaging = null)
    {
        CountryOfManufactureValidate(countryOfManufactureId);

        ProductVariantIdValidate(productVariantId);

        var trimmedDescription = DescriptionValidate(description);
        
        var detail = new ProductDetail()
        {
            Description = trimmedDescription,
            CreatedAt = now,
            CountryOfManufactureId = countryOfManufactureId,
            ProductVariantId = productVariantId,
            IsDeleted = false
        };

        var trimmedModelFeatures = ModelFeaturesValidation(modelFeatures);
        
        if(!string.IsNullOrEmpty(trimmedModelFeatures))
            detail.ModelFeatures = trimmedModelFeatures;
        
        var trimmedDecorativeElements = DecorativeElementsValidation(decorativeElements);
        
        if (!string.IsNullOrEmpty(trimmedDecorativeElements))
            detail.DecorativeElements = trimmedDecorativeElements;

        var trimmedEquipment = EquipmentValidation(equipment);
        
        if (!string.IsNullOrEmpty(equipment))
            detail.Equipment = trimmedEquipment;

        var trimmedComposition = CompositionValidation(composition);
        
        if (!string.IsNullOrEmpty(composition))
            detail.Composition = trimmedComposition;

        var trimmedCaringOfThings = CaringOfThingsValidation(caringOfThings);
        
        if (!string.IsNullOrEmpty(caringOfThings))
            detail.CaringOfThings = trimmedCaringOfThings;
        
        if (typeOfPackaging.HasValue)
            detail.TypeOfPacking = typeOfPackaging;
        
        return detail;
    }

    public static ProductDetail Reconstitute(
        Guid id,
        int countryOfManufactureId,
        Guid productVariantId,
        string description,
        string modelFeatures,
        string decorativeElements,
        string equipment,
        string composition,
        string caringOfThings,
        TypeOfPackaging? typeOfPackaging,
        bool isDeleted,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt,
        DateTimeOffset deletedAt)
    {
        var detail = new ProductDetail()
        {
            Id = id,
            Description = description,
            ModelFeatures = modelFeatures,
            DecorativeElements = decorativeElements,
            Equipment = equipment,
            Composition = composition,
            CaringOfThings = caringOfThings,
            TypeOfPacking = typeOfPackaging,
            CountryOfManufactureId = countryOfManufactureId,
            ProductVariantId = productVariantId,
            IsDeleted = isDeleted,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt,
        };

        return detail;
    }

    public void ChangeDescription(
        DateTimeOffset now,
        string description)
    {
        EnsureNotDeleted();
        
        var trimmedDescription = DescriptionValidate(description);
        
        if(Description == trimmedDescription) return;

        Description = trimmedDescription;
        UpdatedAt = now;
    }
    
    public void ChangeModelFeatures(
        DateTimeOffset now,
        string modelFeatures)
    {
        EnsureNotDeleted();
        
        var trimmedModelFeatures = ModelFeaturesValidation(modelFeatures);
        
        if(ModelFeatures == trimmedModelFeatures) return;

        ModelFeatures = trimmedModelFeatures;
        UpdatedAt = now;
    }
    
    public void ChangeDecorativeElements(
        DateTimeOffset now,
        string decorativeElements)
    {
        EnsureNotDeleted();
        
        var trimmedDecorativeElements = DecorativeElementsValidation(decorativeElements);
        
        if(string.IsNullOrEmpty(trimmedDecorativeElements))
            throw new DomainException("Product Detail decorative elements cannot be null or whitespace.");
        
        if(DecorativeElements == trimmedDecorativeElements) return;

        DecorativeElements = trimmedDecorativeElements;
        UpdatedAt = now;
    }
    
    public void ChangeEquipment(
        DateTimeOffset now,
        string equipment)
    {
        EnsureNotDeleted();
        
        var trimmedEquipment = EquipmentValidation(equipment);
        
        if (string.IsNullOrEmpty(trimmedEquipment))
            throw new DomainException("Product Detail equipment cannot be null or whitespace.");
        
        if(Equipment == trimmedEquipment) return;

        Equipment = trimmedEquipment;
        UpdatedAt = now;
    }
    
    public void ChangeComposition(
        DateTimeOffset now,
        string composition)
    {
        EnsureNotDeleted();
        
        var trimmedComposition = CompositionValidation(composition);
        
        if (string.IsNullOrEmpty(composition))
            throw new DomainException("Product Detail composition cannot be null or whitespace.");
        
        if(Composition == trimmedComposition) return;

        Composition = trimmedComposition;
        UpdatedAt = now;
    }
    
    public void ChangeCaringOfThings(
        DateTimeOffset now,
        string caringOfThings)
    {
        EnsureNotDeleted();
        
        var trimmedCaringOfThings = CaringOfThingsValidation(caringOfThings);
        
        if (string.IsNullOrEmpty(caringOfThings))
            throw new DomainException("Product Detail Caring Of Things cannot be null or whitespace.");
        
        if(CaringOfThings == trimmedCaringOfThings) return;

        CaringOfThings = trimmedCaringOfThings;
        UpdatedAt = now;
    }
    
    public void ChangeTypeOfPacking(
        DateTimeOffset now,
        TypeOfPackaging typeOfPackaging)
    {
        EnsureNotDeleted();
        
        TypeOfPacking = typeOfPackaging;
        UpdatedAt = now;
    }
    
    public void ChangeCountryOfManufactureId(
        DateTimeOffset now,
        int countryOfManufactureId)
    {
        EnsureNotDeleted();
        
        if(countryOfManufactureId <= 0)
            throw new DomainException("Product Detail country of manufacture ID must be more then 0.");
        
        if(CountryOfManufactureId == countryOfManufactureId) return;

        CountryOfManufactureId = countryOfManufactureId;
        UpdatedAt = now;
    }
    
    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Entity is deleted.");
    }

    private static void CountryOfManufactureValidate(int countryOfManufactureId)
    {
        if(countryOfManufactureId <= 0)
            throw new DomainException("Product Detail country of manufacture ID must be more then 0.");
    }
    
    private static void ProductVariantIdValidate(Guid productVariantId)
    {
        if(productVariantId == Guid.Empty)
            throw new DomainException("Product Detail product variant id cannot be guid empty.");
    }
    
    private static string DescriptionValidate(string description)
    {
        if(string.IsNullOrWhiteSpace(description))
            throw new DomainException("Product Detail Description cannot be null or whitespace.");
        
        var trimmedDescription = description.Trim();
        
        if(trimmedDescription.Length is > MaxDescriptionLength or < MinDescriptionLength)
            throw new DomainException($"Product Detail Description length must between {MaxDescriptionLength} and {MinDescriptionLength}.");

        return trimmedDescription;
    }

    private static string ModelFeaturesValidation(string? modelFeatures)
    {
        if (string.IsNullOrWhiteSpace(modelFeatures)) 
            return string.Empty;
        
        var trimmedModelFeatures = modelFeatures.Trim();
        
        if(trimmedModelFeatures.Length is > MaxModelFeaturesLength or < MinModelFeaturesLength)
            throw new DomainException($"Product Detail model features length must between {MaxModelFeaturesLength} and {MinModelFeaturesLength}.");

        return trimmedModelFeatures;
    }
    
    private static string DecorativeElementsValidation(string? decorativeElements)
    {
        if(string.IsNullOrWhiteSpace(decorativeElements))
            return string.Empty;
        
        var trimmedDecorativeElements = decorativeElements.Trim();
        
        if(trimmedDecorativeElements.Length is > MaxDecorativeElementsLength or < MinDecorativeElementsLength)
            throw new DomainException($"Product Detail decorative elements length must between {MaxDecorativeElementsLength} and {MinDecorativeElementsLength}.");

        return trimmedDecorativeElements;
    }
    
    private static string EquipmentValidation(string? equipment)
    {
        if(string.IsNullOrWhiteSpace(equipment))
            return string.Empty;
        
        var trimmedEquipment = equipment.Trim();
        
        if(trimmedEquipment.Length is > MaxEquipmentLength or < MinEquipmentLength)
            throw new DomainException($"Product Detail equipment length must between {MaxEquipmentLength} and {MinEquipmentLength}.");

        return trimmedEquipment;
    }
    
    private static string CompositionValidation(string? composition)
    {
        if(string.IsNullOrWhiteSpace(composition))
            return string.Empty;
        
        var trimmedComposition = composition.Trim();
        
        if(trimmedComposition.Length is > MaxCompositionLength or < MinCompositionLength)
            throw new DomainException($"Product Detail composition length must between {MaxCompositionLength} and {MinCompositionLength}.");

        return trimmedComposition;
    }
    
    private static string CaringOfThingsValidation(string? caringOfThings)
    {
        if(string.IsNullOrWhiteSpace(caringOfThings))
            return string.Empty;
        
        var trimmedCaringOfThings = caringOfThings.Trim();
        
        if(trimmedCaringOfThings.Length is > MaxCaringOfThingsLength or < MinCaringOfThingsLength)
            throw new DomainException($"Product Detail caring of things length must between {MaxCaringOfThingsLength} and {MinCaringOfThingsLength}.");

        return trimmedCaringOfThings;
    }
}