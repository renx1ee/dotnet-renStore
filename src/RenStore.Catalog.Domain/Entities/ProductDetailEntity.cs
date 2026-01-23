using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a product Detail physical entity with lifecycle and invariants.
/// </summary>
public class ProductDetailEntity
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
    
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    
    public ProductVariant? ProductVariant { get; private set; }
    public Guid ProductVariantId { get; private set; }

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
    
    private ProductDetailEntity() { }

    public static ProductDetailEntity Create(
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
        if(countryOfManufactureId <= 0)
            throw new DomainException("Product Detail country of manufacture ID must be more then 0.");
        
        if(productVariantId == Guid.Empty)
            throw new DomainException("Product Detail product variant id cannot be guid empty.");
        
        if(string.IsNullOrWhiteSpace(description))
            throw new DomainException("Product Detail Description cannot be null or whitespace.");
        
        var trimmedDescription = description.Trim();
        
        if(trimmedDescription.Length is > MaxDescriptionLength or < MinDescriptionLength)
            throw new DomainException($"Product Detail Description length must between {MaxDescriptionLength} and {MinDescriptionLength}.");
        
        var detail = new ProductDetailEntity()
        {
            Description = trimmedDescription,
            CreatedAt = now,
            CountryOfManufactureId = countryOfManufactureId,
            ProductVariantId = productVariantId,
            IsDeleted = false
        };

        if (!string.IsNullOrWhiteSpace(modelFeatures))
        {
            var trimmedModelFeatures = modelFeatures.Trim();
        
            if(trimmedModelFeatures.Length is > MaxModelFeaturesLength or < MinModelFeaturesLength)
                throw new DomainException($"Product Detail model features length must between {MaxModelFeaturesLength} and {MinModelFeaturesLength}.");

            detail.ModelFeatures = trimmedModelFeatures;
        }
        
        if (!string.IsNullOrWhiteSpace(decorativeElements))
        {
            var trimmedDecorativeElements = decorativeElements.Trim();
        
            if(trimmedDecorativeElements.Length is > MaxDecorativeElementsLength or < MinDecorativeElementsLength)
                throw new DomainException($"Product Detail decorative elements length must between {MaxDecorativeElementsLength} and {MinDecorativeElementsLength}.");

            detail.DecorativeElements = trimmedDecorativeElements;
        }
        
        if (!string.IsNullOrWhiteSpace(equipment))
        {
            var trimmedEquipment = equipment.Trim();
        
            if(trimmedEquipment.Length is > MaxEquipmentLength or < MinEquipmentLength)
                throw new DomainException($"Product Detail equipment length must between {MaxEquipmentLength} and {MinEquipmentLength}.");

            detail.Equipment = trimmedEquipment;
        }
        
        if (!string.IsNullOrWhiteSpace(composition))
        {
            var trimmedComposition = composition.Trim();
        
            if(trimmedComposition.Length is > MaxCompositionLength or < MinCompositionLength)
                throw new DomainException($"Product Detail composition length must between {MaxCompositionLength} and {MinCompositionLength}.");

            detail.Composition = trimmedComposition;
        }
        
        if (!string.IsNullOrWhiteSpace(caringOfThings))
        {
            var trimmedCaringOfThings = caringOfThings.Trim();
        
            if(trimmedCaringOfThings.Length is > MaxCaringOfThingsLength or < MinCaringOfThingsLength)
                throw new DomainException($"Product Detail caring of things length must between {MaxCaringOfThingsLength} and {MinCaringOfThingsLength}.");

            detail.CaringOfThings = trimmedCaringOfThings;
        }

        if (typeOfPackaging.HasValue)
            detail.TypeOfPacking = typeOfPackaging;
        
        return detail;
    }

    public void ChangeDescription(
        DateTimeOffset now,
        string description)
    {
        EnsureNotDeleted();
        
        if(string.IsNullOrWhiteSpace(description))
            throw new DomainException("Product Detail Description cannot be null or whitespace.");
        
        var trimmedDescription = description.Trim();
        
        if(trimmedDescription.Length is > MaxDescriptionLength or < MinDescriptionLength)
            throw new DomainException($"Product Detail Description length must between {MaxDescriptionLength} and {MinDescriptionLength}.");
        
        if(Description == trimmedDescription) return;

        Description = trimmedDescription;
        UpdatedAt = now;
    }
    
    public void ChangeModelFeatures(
        DateTimeOffset now,
        string modelFeatures)
    {
        EnsureNotDeleted();
        
        if(string.IsNullOrWhiteSpace(modelFeatures))
            throw new DomainException("Product Detail model features cannot be null or whitespace.");
        
        var trimmedModelFeatures = modelFeatures.Trim();
        
        if(trimmedModelFeatures.Length is > MaxModelFeaturesLength or < MinModelFeaturesLength)
            throw new DomainException($"Product Detail model features length must between {MaxModelFeaturesLength} and {MinModelFeaturesLength}.");
        
        if(ModelFeatures == trimmedModelFeatures) return;

        ModelFeatures = trimmedModelFeatures;
        UpdatedAt = now;
    }
    
    public void ChangeDecorativeElements(
        DateTimeOffset now,
        string decorativeElements)
    {
        EnsureNotDeleted();
        
        if(string.IsNullOrWhiteSpace(decorativeElements))
            throw new DomainException("Product Detail decorative elements cannot be null or whitespace.");
        
        var trimmedDecorativeElements = decorativeElements.Trim();
        
        if(trimmedDecorativeElements.Length is > MaxDecorativeElementsLength or < MinDecorativeElementsLength)
            throw new DomainException($"Product Detail decorative elements length must between {MaxDecorativeElementsLength} and {MinDecorativeElementsLength}.");
        
        if(DecorativeElements == trimmedDecorativeElements) return;

        DecorativeElements = trimmedDecorativeElements;
        UpdatedAt = now;
    }
    
    public void ChangeEquipment(
        DateTimeOffset now,
        string equipment)
    {
        EnsureNotDeleted();
        
        if(string.IsNullOrWhiteSpace(equipment))
            throw new DomainException("Product Detail equipment cannot be null or whitespace.");
        
        var trimmedEquipment = equipment.Trim();
        
        if(trimmedEquipment.Length is > MaxEquipmentLength or < MinEquipmentLength)
            throw new DomainException($"Product Detail equipment length must between {MaxEquipmentLength} and {MinEquipmentLength}.");
        
        if(Equipment == trimmedEquipment) return;

        Equipment = trimmedEquipment;
        UpdatedAt = now;
    }
    
    public void ChangeComposition(
        DateTimeOffset now,
        string composition)
    {
        EnsureNotDeleted();
        
        if(string.IsNullOrWhiteSpace(composition))
            throw new DomainException("Product Detail composition cannot be null or whitespace.");
        
        var trimmedComposition = composition.Trim();
        
        if(trimmedComposition.Length is > MaxCompositionLength or < MinCompositionLength)
            throw new DomainException($"Product Detail composition length must between {MaxCompositionLength} and {MinCompositionLength}.");
        
        if(Composition == trimmedComposition) return;

        Composition = trimmedComposition;
        UpdatedAt = now;
    }
    
    public void ChangeCaringOfThings(
        DateTimeOffset now,
        string caringOfThings)
    {
        EnsureNotDeleted();
        
        if(string.IsNullOrWhiteSpace(caringOfThings))
            throw new DomainException("Product Detail Caring Of Things cannot be null or whitespace.");
        
        var trimmedCaringOfThings = caringOfThings.Trim();
        
        if(trimmedCaringOfThings.Length is > MaxCaringOfThingsLength or < MinCaringOfThingsLength)
            throw new DomainException($"Product Detail Caring Of Things length must between {MaxCaringOfThingsLength} and {MinCaringOfThingsLength}.");
        
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

        CountryOfManufactureId = countryOfManufactureId;
        UpdatedAt = now;
    }

    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        IsDeleted = true;
        DeletedAt = now;
        UpdatedAt = now;
    }
    
    public void Restore(DateTimeOffset now)
    {
        if(!IsDeleted)
            throw new DomainException("Product Detail not was deleted.");

        IsDeleted = false;
        UpdatedAt = now;
        DeletedAt = null;
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new DomainException("Product Detail already was deleted.");
    }
}