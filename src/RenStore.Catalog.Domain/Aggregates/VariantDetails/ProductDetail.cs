using RenStore.Catalog.Domain.Aggregates.Variant.Rules;
using RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails;

/// <summary>
/// Represents a product Detail physical entity with lifecycle and invariants.
/// </summary>
public class ProductDetail
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } 
    public string Composition { get; private set; }
    public string? ModelFeatures { get; private set; }
    public string? DecorativeElements { get; private set; } 
    public string? Equipment { get; private set; }
    public string? CaringOfThings { get; private set; } 
    public TypeOfPackaging? TypeOfPacking { get; private set; }
    public int CountryOfManufactureId { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    
    private ProductDetail() { }

    public static ProductDetail Create(
        DateTimeOffset now,
        Guid productVariantId,
        int countryOfManufactureId,
        string description,
        string composition,
        string? caringOfThings = null,
        TypeOfPackaging? typeOfPackaging = null,
        string? modelFeatures = null,
        string? decorativeElements = null,
        string? equipment = null)
    {
        ProductDetailRules.CountryOfManufactureValidate(countryOfManufactureId);
        ProductDetailRules.ProductVariantIdValidate(productVariantId);

        var trimmedDescription = ProductDetailRules.DescriptionNormalizedAndValidate(description);
        var trimmedComposition = ProductDetailRules.CompositionNormalizedAndValidate(composition);
        var trimmedModelFeatures = ProductDetailRules.ModelFeaturesNormalizedAndValidate(modelFeatures);
        var trimmedDecorativeElements = ProductDetailRules.DecorativeElementsNormalizedAndValidate(decorativeElements);
        var trimmedEquipment = ProductDetailRules.EquipmentNormalizedAndValidate(equipment);
        var trimmedCaringOfThings = ProductDetailRules.CaringOfThingsNormalizedAndValidate(caringOfThings);

        var detailId = Guid.NewGuid();

        var details = new ProductDetail();

        details.Raise(new VariantDetailsCreated(
            OccurredAt: now,
            Id: detailId,
            VariantId: productVariantId,
            CountryOfManufactureId: countryOfManufactureId,
            ModelFeatures: trimmedModelFeatures,
            DecorativeElements: trimmedDecorativeElements,
            Equipment: trimmedEquipment,
            Description: trimmedDescription,
            Composition: trimmedComposition,
            CaringOfThings: trimmedCaringOfThings,
            TypeOfPackaging: typeOfPackaging ?? null));

        return details;
    }
    
    public void ChangeDetailDescription(
        DateTimeOffset now,
        Guid variantId,
        string description)
    {
        var trimmedDescription = ProductDetailRules
            .DescriptionNormalizedAndValidate(description);
        
        if(Description == trimmedDescription) return;
        
        Raise(new VariantDetailsDescriptionUpdated(
            OccurredAt: now,
            VariantId: variantId,
            Description: trimmedDescription));
    }

    public void ChangeDetailModelFeatures(
        DateTimeOffset now,
        Guid variantId,
        string? modelFeatures)
    {
        var trimmedModelFeatures = ProductDetailRules
            .ModelFeaturesNormalizedAndValidate(modelFeatures);
        
        if(ModelFeatures == trimmedModelFeatures) return;
        
        Raise(new VariantDetailsModelFeaturesUpdated(
            OccurredAt: now,
            VariantId: variantId,
            ModelFeatures: trimmedModelFeatures));
    }

    public void ChangeDetailDecorativeElements(
        DateTimeOffset now,
        Guid variantId,
        string? decorativeElements)
    {
        var trimmedDecorativeElements = ProductDetailRules
            .DecorativeElementsNormalizedAndValidate(decorativeElements);
        
        if(string.IsNullOrEmpty(trimmedDecorativeElements))
            throw new DomainException("Product Detail decorative elements cannot be null or whitespace.");
        
        if(DecorativeElements == trimmedDecorativeElements) return;
        
        Raise(new VariantDetailsDecorativeElementsUpdated(
            OccurredAt: now,
            VariantId: variantId,
            DecorativeElements: trimmedDecorativeElements));
    }

    public void ChangeDetailEquipment(
        DateTimeOffset now,
        Guid variantId,
        string? equipment)
    {
        var trimmedEquipment = ProductDetailRules
            .EquipmentNormalizedAndValidate(equipment);
        
        if (string.IsNullOrEmpty(trimmedEquipment))
            throw new DomainException("Product Detail equipment cannot be null or whitespace.");
        
        if(Equipment == trimmedEquipment) return;
        
        Raise(new VariantDetailsEquipmentUpdated(
            OccurredAt: now,
            VariantId: variantId,
            Equipment: trimmedEquipment));
    }

    public void ChangeDetailComposition(
        DateTimeOffset now,
        Guid variantId,
        string composition)
    {
        var trimmedComposition = ProductDetailRules
            .CompositionNormalizedAndValidate(composition);
        
        if (string.IsNullOrEmpty(composition))
            throw new DomainException("Product Detail composition cannot be null or whitespace.");
        
        if(Composition == trimmedComposition) return;
        
        Raise(new VariantDetailsCompositionUpdated(
            OccurredAt: now,
            VariantId: variantId,
            Composition: trimmedComposition));
    }

    public void ChangeDetailCaringOfThings(
        DateTimeOffset now,
        Guid variantId,
        string? caringOfThings)
    {
        var trimmedCaringOfThings = ProductDetailRules
            .CaringOfThingsNormalizedAndValidate(caringOfThings);
        
        if (string.IsNullOrEmpty(caringOfThings))
            throw new DomainException("Product Detail Caring Of Things cannot be null or whitespace.");
        
        if(CaringOfThings == trimmedCaringOfThings) return;
        
        Raise(new VariantDetailsCaringOfThingsUpdated(
            OccurredAt: now,
            VariantId: variantId,
            CaringOfThings: trimmedCaringOfThings));
    }

    public void ChangeDetailTypeOfPacking(
        DateTimeOffset now,
        Guid variantId,
        TypeOfPackaging typeOfPackaging)
    {
        Raise(new VariantDetailsTypeOfPackingUpdated(
            OccurredAt: now,
            VariantId: variantId,
            TypeOfPackaging: typeOfPackaging));
    }

    public void ChangeCountryOfManufactureId(
        DateTimeOffset now,
        Guid variantId,
        int countryOfManufactureId)
    {
        if(countryOfManufactureId <= 0)
            throw new DomainException("Product Detail country of manufacture ID must be more then 0.");
        
        if(CountryOfManufactureId == countryOfManufactureId) 
            return;
        
        Raise(new VariantDetailsCountryOfManufactureIdUpdated(
            OccurredAt: now,
            VariantId: variantId,
            CountryOfManufactureId: countryOfManufactureId));
    }
    
    protected override void Apply(object @event)
    {
        switch (@event)
        {
            case VariantDetailsCreated e:
                Id = e.Id;
                CreatedAt = e.OccurredAt;
                CountryOfManufactureId = e.CountryOfManufactureId;
                ProductVariantId = e.VariantId;
                Description = e.Description;
                Composition = e.Composition;
                CaringOfThings = e.CaringOfThings;
                TypeOfPacking = e.TypeOfPackaging; 
                ModelFeatures = e.ModelFeatures; 
                DecorativeElements = e.DecorativeElements;
                Equipment = e.Equipment;
                break;
            
            case VariantDetailsDescriptionUpdated e:
                Description = e.Description;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsCaringOfThingsUpdated e:
                CaringOfThings = e.CaringOfThings;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsCompositionUpdated e:
                Composition = e.Composition;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsCountryOfManufactureIdUpdated e:
                CountryOfManufactureId = e.CountryOfManufactureId;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsDecorativeElementsUpdated e:
                DecorativeElements = e.DecorativeElements;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsEquipmentUpdated e:
                Equipment = e.Equipment;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsModelFeaturesUpdated e:
                ModelFeatures = e.ModelFeatures;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsTypeOfPackingUpdated e:
                TypeOfPacking = e.TypeOfPackaging;
                UpdatedAt = e.OccurredAt;
                break;
        }
    }
}

/*
 * public static ProductDetail Reconstitute(
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
 */