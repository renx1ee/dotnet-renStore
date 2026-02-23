using RenStore.Catalog.Domain.Aggregates.Variant.Rules;
using RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails;

/// <summary>
/// Represents a product Detail physical entity with lifecycle and invariants.
/// </summary>
public class VariantDetail
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    public Guid Id { get; private set; }
    public string Description { get; private set; } 
    public string Composition { get; private set; }
    public string? ModelFeatures { get; private set; }
    public string? DecorativeElements { get; private set; } 
    public string? Equipment { get; private set; }
    public string? CaringOfThings { get; private set; } 
    public TypeOfPacking? TypeOfPacking { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public int CountryOfManufactureId { get; private set; }
    public Guid VariantId { get; private set; }
    
    private VariantDetail() { }

    public static VariantDetail Create(
        DateTimeOffset now,
        Guid variantId,
        int countryOfManufactureId,
        string description,
        string composition,
        string? caringOfThings = null,
        TypeOfPacking? typeOfPackaging = null,
        string? modelFeatures = null,
        string? decorativeElements = null,
        string? equipment = null)
    {
        ProductDetailRules.CountryOfManufactureValidate(countryOfManufactureId);
        ProductDetailRules.ProductVariantIdValidate(variantId);

        var trimmedDescription              = ProductDetailRules.DescriptionNormalizedAndValidate(description);
        var trimmedComposition        = ProductDetailRules.CompositionNormalizedAndValidate(composition);
        
        var trimmedModelFeatures      = ProductDetailRules.ModelFeaturesNormalizedAndValidate(modelFeatures);
        var trimmedDecorativeElements = ProductDetailRules.DecorativeElementsNormalizedAndValidate(decorativeElements);
        var trimmedEquipment          = ProductDetailRules.EquipmentNormalizedAndValidate(equipment);
        var trimmedCaringOfThings     = ProductDetailRules.CaringOfThingsNormalizedAndValidate(caringOfThings);

        var detailId = Guid.NewGuid();
        var details = new VariantDetail();

        details.Raise(new VariantDetailsCreated(
            OccurredAt: now,
            DetailId: detailId,
            VariantId: variantId,
            CountryOfManufactureId: countryOfManufactureId,
            ModelFeatures: trimmedModelFeatures,
            DecorativeElements: trimmedDecorativeElements,
            Equipment: trimmedEquipment,
            Description: trimmedDescription,
            Composition: trimmedComposition,
            CaringOfThings: trimmedCaringOfThings,
            TypeOfPackaging: typeOfPackaging));

        return details;
    }
    
    public void ChangeDescription(
        DateTimeOffset now,
        string description)
    {
        var trimmedDescription = ProductDetailRules
            .DescriptionNormalizedAndValidate(description);
        
        if(Description == trimmedDescription) return;
        
        Raise(new VariantDetailsDescriptionUpdated(
            OccurredAt: now,
            DetailId: Id,
            Description: trimmedDescription));
    }

    public void ChangeModelFeatures(
        DateTimeOffset now,
        string modelFeatures)
    {
        var trimmedModelFeatures = ProductDetailRules
            .ModelFeaturesNormalizedAndValidate(modelFeatures);
            
        if (string.IsNullOrEmpty(trimmedModelFeatures))
            throw new DomainException(
                "Model features cannot be null.");
        
        if(ModelFeatures == trimmedModelFeatures) return;
        
        Raise(new VariantDetailsModelFeaturesUpdated(
            OccurredAt: now,
            ModelFeatures: trimmedModelFeatures));
    }

    public void ChangeDecorativeElements(
        DateTimeOffset now,
        string? decorativeElements)
    {
        var trimmedDecorativeElements = ProductDetailRules
            .DecorativeElementsNormalizedAndValidate(decorativeElements);
        
        if(string.IsNullOrEmpty(trimmedDecorativeElements))
            throw new DomainException(
                "Product Detail decorative elements cannot be null or whitespace.");
        
        if(DecorativeElements == trimmedDecorativeElements) return;
        
        Raise(new VariantDetailsDecorativeElementsUpdated(
            OccurredAt: now,
            DecorativeElements: trimmedDecorativeElements));
    }

    public void ChangeEquipment(
        DateTimeOffset now,
        string? equipment)
    {
        var trimmedEquipment = ProductDetailRules
            .EquipmentNormalizedAndValidate(equipment);
        
        if (string.IsNullOrEmpty(trimmedEquipment))
            throw new DomainException(
                "Product Detail equipment cannot be null or whitespace.");
        
        if(Equipment == trimmedEquipment) return;
        
        Raise(new VariantDetailsEquipmentUpdated(
            OccurredAt: now,
            Equipment: trimmedEquipment));
    }

    public void ChangeComposition(
        DateTimeOffset now,
        string composition)
    {
        var trimmedComposition = ProductDetailRules
            .CompositionNormalizedAndValidate(composition);
        
        if (string.IsNullOrEmpty(trimmedComposition))
            throw new DomainException(
                "Product Detail composition cannot be null or whitespace.");
        
        if(Composition == trimmedComposition) return;
        
        Raise(new VariantDetailsCompositionUpdated(
            OccurredAt: now,
            Composition: trimmedComposition));
    }

    public void ChangeCaringOfThings(
        DateTimeOffset now,
        string? caringOfThings)
    {
        var trimmedCaringOfThings = ProductDetailRules
            .CaringOfThingsNormalizedAndValidate(caringOfThings);
        
        if (string.IsNullOrEmpty(trimmedCaringOfThings))
            throw new DomainException(
                "Product Detail Caring Of Things cannot be null or whitespace.");
        
        if(CaringOfThings == trimmedCaringOfThings) return;
        
        Raise(new VariantDetailsCaringOfThingsUpdated(
            OccurredAt: now,
            CaringOfThings: trimmedCaringOfThings));
    }

    public void ChangeTypeOfPacking(
        DateTimeOffset now,
        TypeOfPacking typeOfPacking)
    {
        if (TypeOfPacking == typeOfPacking) return;
        
        Raise(new VariantDetailsTypeOfPackingUpdated(
            OccurredAt: now,
            TypeOfPacking: typeOfPacking));
    }

    public void ChangeCountryOfManufactureId(
        DateTimeOffset now,
        int countryOfManufactureId)
    {
        if(countryOfManufactureId <= 0)
            throw new DomainException(
                "Product Detail country of manufacture ID must be more then 0.");
        
        if(CountryOfManufactureId == countryOfManufactureId) 
            return;
        
        Raise(new VariantDetailsCountryOfManufactureIdUpdated(
            OccurredAt: now,
            CountryOfManufactureId: countryOfManufactureId));
    }
    
    protected override void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case VariantDetailsCreated e:
                Id = e.DetailId;
                CreatedAt = e.OccurredAt;
                CountryOfManufactureId = e.CountryOfManufactureId;
                VariantId = e.VariantId;
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
                TypeOfPacking = e.TypeOfPacking;
                UpdatedAt = e.OccurredAt;
                break;
        }
    }
    
    public static VariantDetail Rehydrate(IEnumerable<IDomainEvent> history)
    {
        var variantDetail = new VariantDetail();
        
        foreach (var @event in history)
        {
            variantDetail.Apply(@event);
            variantDetail.Version++;
        }

        return variantDetail;
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