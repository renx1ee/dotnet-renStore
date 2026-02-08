using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

/// <summary>
/// Represents a product Detail physical entity with lifecycle and invariants.
/// </summary>
public class ProductDetail
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } 
    public string? ModelFeatures { get; private set; }
    public string? DecorativeElements { get; private set; } 
    public string? Equipment { get; private set; }
    public string Composition { get; private set; }
    public string? CaringOfThings { get; private set; } 
    public TypeOfPackaging? TypeOfPacking { get; private set; }
    public int CountryOfManufactureId { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    
    private ProductDetail() { }

    internal static ProductDetail Create(
        Guid id,
        DateTimeOffset now,
        int countryOfManufactureId,
        Guid productVariantId,
        string description,
        string composition,
        string? caringOfThings = null,
        TypeOfPackaging? typeOfPackaging = null,
        string? modelFeatures = null,
        string? decorativeElements = null,
        string? equipment = null)
    {
        return new ProductDetail()
        {
            Id = id,
            Description = description, 
            CreatedAt = now,
            CountryOfManufactureId = countryOfManufactureId,
            ProductVariantId = productVariantId,
            Composition = composition,
            CaringOfThings = caringOfThings,
            TypeOfPacking = typeOfPackaging,
            ModelFeatures = modelFeatures,
            DecorativeElements = decorativeElements,
            Equipment = equipment,
            IsDeleted = false
        };
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
        
        var trimmedDescription = ProductDetailRules.DescriptionNormalizedAndValidate(description);
        
        if(Description == trimmedDescription) return;

        Description = trimmedDescription;
        UpdatedAt = now;
    }
    
    public void ChangeModelFeatures(
        DateTimeOffset now,
        string modelFeatures)
    {
        EnsureNotDeleted();
        
        var trimmedModelFeatures = ProductDetailRules.ModelFeaturesNormalizedAndValidate(modelFeatures);
        
        if(ModelFeatures == trimmedModelFeatures) return;

        ModelFeatures = trimmedModelFeatures;
        UpdatedAt = now;
    }
    
    public void ChangeDecorativeElements(
        DateTimeOffset now,
        string decorativeElements)
    {
        EnsureNotDeleted();
        
        var trimmedDecorativeElements = ProductDetailRules.DecorativeElementsNormalizedAndValidate(decorativeElements);
        
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
        
        var trimmedEquipment = ProductDetailRules.EquipmentNormalizedAndValidate(equipment);
        
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
        
        var trimmedComposition = ProductDetailRules.CompositionNormalizedAndValidate(composition);
        
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
        
        var trimmedCaringOfThings = ProductDetailRules.CaringOfThingsNormalizedAndValidate(caringOfThings);
        
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

    public record CreateVariantDetailsData
    (
        int CountryOfManufactureId,
        Guid ProductVariantId,
        string Description,
        string? ModelFeatures = null,
        string? DecorativeElements = null,
        string? Equipment = null,
        string? Composition = null,
        string? CaringOfThings = null,
        TypeOfPackaging? TypeOfPackaging = null
    );
    
    /// <summary>
    /// Ensures the attribute is not deleted before performing operations.
    /// </summary>
    /// <param name="message">Optional custom error message</param>
    /// <exception cref="DomainException">Thrown when attribute is deleted</exception>
    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Entity is deleted.");
    }
}