using RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Price;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;
using RenStore.Catalog.Domain.Aggregates.Variant.Rules;
using RenStore.Catalog.Domain.Constants;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

/// <summary>
/// Represents a product variant physical entity with lifecycle and invariants.
/// </summary>
public sealed class ProductVariant
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    private readonly List<VariantSize> _sizes = new();
    private readonly List<Guid> _imageIds = new();
    private readonly List<VariantAttribute> _attributes = new();
    
    private Color _color;
    private VariantDetail _details;
    
    /// <summary>
    /// Unique identifier of the product variant.
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Display the name of this specific variant.
    /// Length must be between 25 and 500 characters after trimming.
    /// </summary>
    public string Name { get; private set; }
    
    /// <summary>
    /// Uppercase normalized version of the variant name for case-insensitive operations.
    /// Automatically derived from <see cref="Name"/>.
    /// </summary>
    public string NormalizedName { get; private set; } 
    
    /// <summary>
    /// Internal article number that uniquely identifies this product variant.
    /// </summary>
    public long Article { get; private set; }
    
    /// <summary>
    /// Current lifecycle state on this product variant.
    /// </summary>
    public ProductVariantStatus Status { get; private set; }
    
    /// <summary>
    /// SEO-friendly URL slug for this product variant.
    /// Used to generate permanent links in the catalog and for search engine optimization.
    /// </summary>
    public string Url { get; private set; }
    
    /// <summary>
    /// Image Unique Identifier
    /// </summary>
    public Guid MainImageId { get; private set; }
    
    /// <summary>
    /// Measurement system used for sizing in this product variant.
    /// Determines which size chart applies and how sizes are displayed to customers.
    /// </summary>
    public SizeSystem SizeSystem { get; private set; }
    
    /// <summary>
    /// Category of sizing applicable to this product variant.
    /// Determines which size ranges and conversion tables are relevant.
    /// </summary>
    public SizeType SizeType { get; private set; }
    
    /// <summary>
    /// Date when the product was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }
    
    /// <summary>
    /// Date when the product was updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }
    
    /// <summary>
    /// Date when the product was deleted.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; private set; }
    
    public Guid UpdatedById { get; private set; } 
    public string UpdatedByRole { get; private set; } 
    
    /// <summary>
    /// Unique identifier of the product.
    /// </summary>
    public Guid ProductId { get; private set; }
    
    /// <summary>
    /// Unique identifier of the color.
    /// </summary>
    public int ColorId { get; private set; }
    
    /// <summary>
    /// The collection of sizes associated with this variant.
    /// </summary>
    public IReadOnlyCollection<VariantSize> Sizes => _sizes.AsReadOnly();

    public VariantDetail? Details => _details;
    
    /// <summary>
    /// The collection of images IDs associated with this variant.
    /// </summary>
    public IReadOnlyCollection<Guid> ImageIds => _imageIds.AsReadOnly();
    
    /// <summary>
    /// The collection of attributes associated with this variant.
    /// </summary>
    public IReadOnlyCollection<VariantAttribute> Attributes => _attributes.AsReadOnly();
    
    private ProductVariant() { }

    public static ProductVariant Create(
        DateTimeOffset now,
        Guid productId,
        int colorId,
        string name,
        SizeSystem sizeSystem,
        SizeType sizeType,
        long article,
        string url)
    {
        ProductVariantRules.ValidateProductId(productId);
        ProductVariantRules.ValidateColorId(colorId);
        ProductVariantRules.ValidateArticle(article);

        var trimmedName = ProductVariantRules.ValidateAndTrimName(name);
        string trimmedUrl = ProductVariantRules.ValidateAndTrimUrl(url);

        var normalizedName = trimmedName.ToUpperInvariant();
        
        var variantId = Guid.NewGuid();
        var variant = new ProductVariant();
        
        variant.Raise(
            new VariantCreatedEvent(
                EventId: Guid.NewGuid(), 
                VariantId: variantId,
                ProductId: productId,
                ColorId: colorId,
                Name: trimmedName,
                NormalizedName: normalizedName,
                Url: trimmedUrl,
                SizeSystem: sizeSystem,
                SizeType: sizeType,
                Article: article,
                Status: ProductVariantStatus.Draft,
                OccurredAt: now));
        
        return variant;
    }
    
    public void AddDetails(
        DateTimeOffset now,
        string countryOfManufacture,
        string description,
        string composition,
        string? caringOfThings = null,
        TypeOfPacking? typeOfPacking = null,
        string? modelFeatures = null,
        string? decorativeElements = null,
        string? equipment = null)
    {
        if (_details != null)
            throw new DomainException(
                "Variant Details already exists.");
        
        CountryOfManufacture.NameValidation(countryOfManufacture);

        var trimmedDescription              = ProductDetailRules.DescriptionNormalizedAndValidate(description);
        var trimmedComposition         = ProductDetailRules.CompositionNormalizedAndValidate(composition);
        
        var trimmedModelFeatures      = ProductDetailRules.ModelFeaturesNormalizedAndValidate(modelFeatures);
        var trimmedDecorativeElements = ProductDetailRules.DecorativeElementsNormalizedAndValidate(decorativeElements);
        var trimmedEquipment          = ProductDetailRules.EquipmentNormalizedAndValidate(equipment);
        var trimmedCaringOfThings     = ProductDetailRules.CaringOfThingsNormalizedAndValidate(caringOfThings);

        var detailId = Guid.NewGuid();

        Raise(new VariantDetailsCreatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            DetailId: detailId,
            VariantId: Id,
            CountryOfManufacture: countryOfManufacture,
            ModelFeatures: trimmedModelFeatures,
            DecorativeElements: trimmedDecorativeElements,
            Equipment: trimmedEquipment,
            Description: trimmedDescription,
            Composition: trimmedComposition,
            CaringOfThings: trimmedCaringOfThings,
            TypeOfPackaging: typeOfPacking));
    }

    public Guid AddAttribute(
        DateTimeOffset now,
        string key,
        string value)
    {
        EnsureNotDeleted();
        
        if(_attributes.Count(x => !x.IsDeleted) >= 
           CatalogConstants.ProductVariant.MaxAttributesCount)
            throw new DomainException(
                $"Count of attributes cannot be more then {CatalogConstants.ProductVariant.MaxAttributesCount}.");

        var normalizedKey = AttributeKey.KeyNormalizeAndValidate(key);
        var normalizedValue = AttributeValue.ValueNormalizeAndValidate(value);

        if (_attributes.Any(x => x.Key.Key == normalizedKey))
            throw new DomainException(
                "Attribute key already exists with this variant.");

        var attributeId = Guid.NewGuid();
        
        Raise(new AttributeCreatedEvent(
            EventId: Guid.NewGuid(),
            VariantId: Id,
            AttributeId: attributeId,
            OccurredAt: now,
            Key: normalizedKey,
            Value: normalizedValue));

        return attributeId;
    }
    
    public Guid AddSize(
        LetterSize letterSize,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        var activeSizes = _sizes
            .Where(s => !s.IsDeleted)
            .ToList();
        
        if (activeSizes.Any(x => x.Size.LetterSize == letterSize))
            throw new DomainException(
                "The size already exits in the system.");
        
        Size.Validate(
            size: letterSize, 
            type: SizeType, 
            system: SizeSystem); 
        
        var sizeId = Guid.NewGuid();
        
        Raise(new VariantSizeCreatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            SizeId: sizeId,
            VariantId: Id,
            LetterSize: letterSize,
            SizeSystem: SizeSystem,
            SizeType: SizeType));
        
        return sizeId;
    }
    
    public Guid AddPriceToSize(
        DateTimeOffset now,
        DateTimeOffset validFrom,
        decimal amount,
        Currency currency,
        Guid sizeId)
    {
        EnsureNotDeleted();
        
        var size = GetSize(sizeId);
        SizeEnsureNotDeleted(size);
        
        PriceHistoryRules.ValidateSizeId(sizeId);
        PriceHistoryRules.ValidatePrice(amount);

        var priceId = Guid.NewGuid();
        
        Raise(new PriceCreatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            EffectiveFrom: validFrom,
            PriceId: priceId,
            PriceAmount: amount,
            Currency: currency,
            SizeId: sizeId));

        return priceId;
    }

    public void AddImageReference(
        DateTimeOffset now,
        Guid imageId)
    {
        EnsureNotDeleted();

        if (imageId == Guid.Empty)
            throw new DomainException(
                "Image ID cannot be empty guid.");

        if (_imageIds.Contains(imageId))
            throw new DomainException(
                "Image ID already contains.");

        Raise(new AddedImageReferenceEvent(
            OccurredAt: now,
            ImageId: imageId,
            VariantId: Id,
            EventId: Guid.NewGuid()));
    }

    public void Publish(DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        EnsureDetailsExists();

        if (Status == ProductVariantStatus.Published)
            return;
        
        if (MainImageId == Guid.Empty)
            throw new DomainException(
                "Product variant must have main image.");
        
        if (!_imageIds.Any())
            throw new DomainException(
                "Product variant must have at least one image.");

        var activeSizes = _sizes
            .Where(s => !s.IsDeleted)
            .ToList();

        if (!activeSizes.Any())
            throw new DomainException(
                "Product variant must have at least one size.");

        foreach (var size in activeSizes)
        {
            var activePrice = size.Prices
                .FirstOrDefault(x => x.IsActive);
            
            if (activePrice is null)
                throw new DomainException(
                    $"Size: {size.Id} has no active price.");

            if (activePrice.Price.Amount <= 0)
                throw new DomainException(
                    $"Size: {size.Id} has invalid price.");
        }
        
        Raise(new VariantPublishedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            VariantId: Id));
    }
    
    public void SetMainImageId(
        DateTimeOffset now,
        Guid imageId)
    {
        if (imageId == Guid.Empty)
            throw new DomainException(
                "Main image ID cannot be guid empty.");
        
        if(!_imageIds.Contains(imageId))
            throw new DomainException(
                "Image ID does not belong to this variant.");
        
        if(imageId == MainImageId) 
            return;
        
        Raise(new MainImageIdSetEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));
    }
    
    public void Archive(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        ProductVariantRules.UpdatedByParametersValidation(
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        if (Status == ProductVariantStatus.Archived) return;
        
        Raise(new VariantArchivedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            VariantId: Id));
    }
    
    public void MarkAsDraft(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        ProductVariantRules.UpdatedByParametersValidation(
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        if (Status == ProductVariantStatus.Draft) return;
        
        Raise(new VariantDraftedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            VariantId: Id));
    }
    
    public void ChangeName(
        DateTimeOffset now,
        string name)
    {
        EnsureNotDeleted();
        
        string trimmedName = ProductVariantRules.ValidateAndTrimName(name);
        
        if (trimmedName == Name) return;

        var normalizedName = trimmedName.ToUpperInvariant();

        Raise(new VariantNameUpdatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            VariantId: Id,
            Name: trimmedName,
            NormalizedName: normalizedName));
    }
    
    public void ChangeDetailsDescription(
        DateTimeOffset now,
        string description)
    {
        EnsureNotDeleted();
        EnsureDetailsExists();
        
        var trimmedDescription = ProductDetailRules
            .DescriptionNormalizedAndValidate(description);
        
        if(_details.Description == trimmedDescription) return;
        
        Raise(new VariantDetailsDescriptionUpdatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            DetailId: _details.Id,
            Description: trimmedDescription));
    }
    
    public void ChangeDetailsModelFeatures(
        DateTimeOffset now,
        string modelFeatures)
    {
        EnsureNotDeleted();
        EnsureDetailsExists();
        
        var trimmedModelFeatures = ProductDetailRules
            .ModelFeaturesNormalizedAndValidate(modelFeatures);
            
        if (string.IsNullOrEmpty(trimmedModelFeatures))
            throw new DomainException(
                "Model features cannot be null.");
        
        if(_details.ModelFeatures == trimmedModelFeatures) return;
        
        Raise(new VariantDetailsModelFeaturesUpdatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            DetailId: _details.Id,
            ModelFeatures: trimmedModelFeatures));
    }
    
    public void ChangeDetailsDecorativeElements(
        DateTimeOffset now,
        string decorativeElements)
    {
        EnsureNotDeleted();
        EnsureDetailsExists();
        
        var trimmedDecorativeElements = ProductDetailRules
            .DecorativeElementsNormalizedAndValidate(decorativeElements);
        
        if(string.IsNullOrEmpty(trimmedDecorativeElements))
            throw new DomainException(
                "Product Detail decorative elements cannot be null or whitespace.");
        
        if(_details.DecorativeElements == trimmedDecorativeElements) return;
        
        Raise(new VariantDetailsDecorativeElementsUpdatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            DetailId: _details.Id,
            DecorativeElements: trimmedDecorativeElements));
    }
    
    public void ChangeDetailsEquipment(
        DateTimeOffset now,
        string equipment)
    {
        EnsureNotDeleted();
        EnsureDetailsExists();
        
        var trimmedEquipment = ProductDetailRules
            .EquipmentNormalizedAndValidate(equipment);
        
        if (string.IsNullOrEmpty(trimmedEquipment))
            throw new DomainException(
                "Product Detail equipment cannot be null or whitespace.");
        
        if(_details.Equipment == trimmedEquipment) return;
        
        Raise(new VariantDetailsEquipmentUpdatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            DetailId: _details.Id,
            Equipment: trimmedEquipment));
    }
    
    public void ChangeDetailsComposition(
        DateTimeOffset now,
        string composition)
    {
        EnsureNotDeleted();
        EnsureDetailsExists();
        
        var trimmedComposition = ProductDetailRules
            .CompositionNormalizedAndValidate(composition);
        
        if (string.IsNullOrEmpty(trimmedComposition))
            throw new DomainException(
                "Product Detail composition cannot be null or whitespace.");
        
        if(_details.Composition == trimmedComposition) return;
        
        Raise(new VariantDetailsCompositionUpdatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            DetailId: _details.Id,
            Composition: trimmedComposition));
    }
    
    public void ChangeDetailsCaringOfThings(
        DateTimeOffset now,
        string caringOfThings)
    {
        EnsureNotDeleted();
        EnsureDetailsExists();
        
        var trimmedCaringOfThings = ProductDetailRules
            .CaringOfThingsNormalizedAndValidate(caringOfThings);
        
        if (string.IsNullOrEmpty(trimmedCaringOfThings))
            throw new DomainException(
                "Product Detail Caring Of Things cannot be null or whitespace.");
        
        if(_details.CaringOfThings == trimmedCaringOfThings) return;
        
        Raise(new VariantDetailsCaringOfThingsUpdatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            CaringOfThings: trimmedCaringOfThings,
            DetailId: _details.Id));
    }
    
    public void ChangeDetailsTypeOfPacking(
        DateTimeOffset now,
        TypeOfPacking typeOfPacking)
    {
        EnsureNotDeleted();
        EnsureDetailsExists();
        
        if (_details.TypeOfPacking == typeOfPacking) return;

        
        Raise(new VariantDetailsTypeOfPackingUpdatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            DetailId: _details.Id,
            TypeOfPacking: typeOfPacking));
    }

    public void ChangeDetailsCountryOfManufactureId(
        DateTimeOffset now,
        string countryOfManufacture)
    {
        EnsureNotDeleted();
        EnsureDetailsExists();
        
        CountryOfManufacture.NameValidation(countryOfManufacture);
        
        if(_details.CountryOfManufacture.Name == countryOfManufacture) return;
        
        Raise(new VariantDetailsCountryOfManufactureIdUpdatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            DetailId: _details.Id,
            CountryOfManufacture: countryOfManufacture));
    }
    
    public void ChangeAttributeKey(
        DateTimeOffset now,
        Guid attributeId,
        string key)
    {
        EnsureNotDeleted();

        var existingAttribute = GetAttribute(attributeId);

        EnsureAttributeNotDeleted(existingAttribute);
        
        var trimmedKey = AttributeKey.KeyNormalizeAndValidate(key);
        
        if(existingAttribute.Key.Key == trimmedKey) return;
        
        Raise(new AttributeKeyUpdatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            AttributeId: attributeId,
            Key: trimmedKey));
    }
    
    public void ChangeAttributeValue(
        DateTimeOffset now,
        Guid attributeId,
        string value)
    {
        EnsureNotDeleted();

        var existingAttribute = GetAttribute(attributeId);

        EnsureAttributeNotDeleted(existingAttribute);
        
        var trimmedValue = AttributeValue.ValueNormalizeAndValidate(value);
        
        if(existingAttribute.Value.Value == trimmedValue) return;
        
        Raise(new AttributeValueUpdatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            AttributeId: attributeId,
            Value: trimmedValue));
    }
    
    public void Delete(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EnsureNotDeleted(
            "Cannot delete already deleted variant.");
        
        ProductVariantRules.UpdatedByParametersValidation(
            updatedById: updatedById,
            updatedByRole: updatedByRole);

        foreach (var size in _sizes.Where(x => !x.IsDeleted))
        {
            Raise(new VariantSizeRemovedEvent(
                UpdatedById: updatedById,
                UpdatedByRole: updatedByRole,
                EventId: Guid.NewGuid(), 
                OccurredAt: now,
                VariantId: Id,
                SizeId: size.Id));
        }
        
        Raise(new VariantRemovedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            VariantId: Id,
            OccurredAt: now));
    }
    
    public void RemoveSize(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now,
        Guid sizeId)
    {
        EnsureNotDeleted();
        
        ProductVariantRules.UpdatedByParametersValidation(
            updatedById: updatedById,
            updatedByRole: updatedByRole);

        var size = GetSize(sizeId);
        SizeEnsureNotDeleted(size);
        
        Raise(new VariantSizeRemovedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            VariantId: Id,
            SizeId: sizeId));
    }
    
    public void RemoveAttribute(
        Guid updatedById,
        string updatedByRole,
        Guid attributeId,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        ProductVariantRules.UpdatedByParametersValidation(
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        var existingAttribute = GetAttribute(attributeId);

        if (existingAttribute.IsDeleted)
            throw new DomainException(
                "Cannot delete already deleted attribute.");
        
        Raise(new AttributeRemovedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            AttributeId: attributeId,
            OccurredAt: now));
    }
    
    public void RemoveImageReference(
        DateTimeOffset now,
        Guid imageId)
    {
        EnsureNotDeleted();

        if (imageId == Guid.Empty)
            throw new DomainException(
                "Image ID cannot be empty guid.");
        
        if (imageId == MainImageId)
            throw new DomainException(
                "Cannot delete main image.");

        if (!_imageIds.Contains(imageId))
            throw new DomainException(
                "Image does not exists.");

        Raise(new RemoveImageReferenceEvent(
            OccurredAt: now,
            ImageId: imageId,
            VariantId: Id,
            EventId: Guid.NewGuid()));
    }
    
    public void Restore(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        if(Status != ProductVariantStatus.Deleted)
            throw new DomainException(
                "Variant is not deleted.");
        
        ProductVariantRules.UpdatedByParametersValidation(
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        Raise(new VariantRestoredEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            VariantId: Id));
    }
    
    public void RestoreSize(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now,
        Guid sizeId)
    {
        EnsureNotDeleted();
        
        ProductVariantRules.UpdatedByParametersValidation(
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        var size = GetSize(sizeId);
        
        if(!size.IsDeleted)
            throw new DomainException(
                "Size is not deleted.");
        
        Raise(new VariantSizeRestoredEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            VariantId: Id,
            SizeId: sizeId));
    }
    
    public void RestoreAttribute(
        Guid updatedById,
        string updatedByRole,
        Guid attributeId,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        ProductVariantRules.UpdatedByParametersValidation(
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        if(_attributes.Count(x => !x.IsDeleted) >= 
           CatalogConstants.ProductVariant.MaxAttributesCount)
            throw new DomainException(
                $"Count of attributes cannot be more then {CatalogConstants.ProductVariant.MaxAttributesCount}.");
        
        var existingAttribute = GetAttribute(attributeId);

        if (!existingAttribute.IsDeleted) return;
        
        Raise(new AttributeRestoredEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            AttributeId: attributeId));
    }
    
    protected override void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case VariantCreatedEvent e:
                CreatedAt = e.OccurredAt;
                Id = e.VariantId;
                ProductId = e.ProductId;
                ColorId = e.ColorId;
                Name = e.Name;
                NormalizedName = e.NormalizedName;
                Url = e.Url;
                SizeSystem = e.SizeSystem;
                SizeType = e.SizeType;
                Status = e.Status;
                Article = e.Article;
                break;
            
            case VariantSizeCreatedEvent e:
                _sizes.Add(VariantSize.Create(
                    id: e.SizeId,
                    now: e.OccurredAt,
                    size: Size.Create(
                        size: e.LetterSize, 
                        type: e.SizeType, 
                        system: e.SizeSystem),
                    variantId: e.VariantId));
                UpdatedAt = e.OccurredAt;
                break;
            
            case PriceCreatedEvent e:
                var createdSize = _sizes.SingleOrDefault(x => 
                    x.Id == e.SizeId)
                    ?? throw new DomainException(
                        "Variant size is not found.");
                
                createdSize.CloseCurrentPrice(e.OccurredAt);
                
                createdSize.AddPrice(
                    now: e.OccurredAt,
                    newPrice: new PriceHistory(
                        now: e.OccurredAt,
                        startDate: e.EffectiveFrom,
                        priceId: e.PriceId,
                        sizeId: e.SizeId,
                        price: e.PriceAmount,
                        currency: e.Currency));
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsCreatedEvent e:
                _details = VariantDetail.Create(
                    detailId: e.DetailId,
                    now: e.OccurredAt,
                    countryOfManufacture: e.CountryOfManufacture,
                    variantId: e.VariantId,
                    description: e.Description,
                    composition: e.Composition,
                    caringOfThings: e.CaringOfThings,
                    typeOfPackaging: e.TypeOfPackaging,
                    modelFeatures: e.ModelFeatures,
                    decorativeElements: e.DecorativeElements,
                    equipment: e.Equipment
                );
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsDescriptionUpdatedEvent e:
                _details.ChangeDescription(
                    now: e.OccurredAt,
                    description: e.Description);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsModelFeaturesUpdatedEvent e:
                _details.ChangeModelFeatures(
                    now: e.OccurredAt,
                    modelFeatures: e.ModelFeatures);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsDecorativeElementsUpdatedEvent e:
                _details.ChangeDecorativeElements(
                    now: e.OccurredAt,
                    decorativeElements: e.DecorativeElements);
                UpdatedAt = e.OccurredAt;
                break; 
            
            case VariantDetailsEquipmentUpdatedEvent e:
                _details.ChangeEquipment(
                    now: e.OccurredAt,
                    equipment: e.Equipment);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsCompositionUpdatedEvent e:
                _details.ChangeComposition(
                    now: e.OccurredAt,
                    composition: e.Composition);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsCaringOfThingsUpdatedEvent e:
                _details.ChangeCaringOfThings(
                    now: e.OccurredAt,
                    caringOfThings: e.CaringOfThings);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsCountryOfManufactureIdUpdatedEvent e:
                _details.ChangeCountryOfManufacture(
                    now: e.OccurredAt,
                    countryOfManufacture: e.CountryOfManufacture);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsTypeOfPackingUpdatedEvent e:
                _details.ChangeTypeOfPacking(
                    now: e.OccurredAt,
                    typeOfPacking: e.TypeOfPacking);
                UpdatedAt = e.OccurredAt;
                break;
            
            case AttributeCreatedEvent e:
                _attributes.Add(VariantAttribute.Create(
                    now: e.OccurredAt,
                    attributeId: e.AttributeId,
                    variantId: e.VariantId,
                    key: e.Key,
                    value: e.Value));
                UpdatedAt = e.OccurredAt;
                break;
            
            case AttributeKeyUpdatedEvent e:
                var attributeForKey = _attributes.SingleOrDefault(x => 
                    x.Id == e.AttributeId)
                    ?? throw new DomainException(
                        "Variant attribute is not found.");
                
                attributeForKey.ChangeKey(
                        now: e.OccurredAt,
                        key: e.Key);
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case AttributeValueUpdatedEvent e:
                var attributeForValue = _attributes.SingleOrDefault(x => 
                    x.Id == e.AttributeId)
                    ?? throw new DomainException(
                        "Variant attribute is not found.");
                
                attributeForValue.ChangeValue(
                    now: e.OccurredAt,
                    value: e.Value);
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case AttributeRemovedEvent e:
                var attributeForDelete = _attributes.SingleOrDefault(x => 
                    x.Id == e.AttributeId)
                    ?? throw new DomainException(
                        "Variant attribute is not found.");
                
                attributeForDelete.Delete(
                    now: e.OccurredAt);
                
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                UpdatedAt = e.OccurredAt;
                break;
            
            case AttributeRestoredEvent e:
                var attributeForRestore = _attributes.SingleOrDefault(x => 
                    x.Id == e.AttributeId)
                    ?? throw new DomainException(
                        "Variant attribute is not found.");
                
                attributeForRestore.Restore(
                    now: e.OccurredAt);
                
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                UpdatedAt = e.OccurredAt;
                break;
            
            case MainImageIdSetEvent e:
                MainImageId = e.ImageId;
                UpdatedAt = e.OccurredAt;
                break;
            
            case AddedImageReferenceEvent e:
                _imageIds.Add(e.ImageId);
                UpdatedAt = e.OccurredAt;
                break;
            
            case RemoveImageReferenceEvent e:
                _imageIds.Remove(e.ImageId);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantArchivedEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                Status = ProductVariantStatus.Archived;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDraftedEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                Status = ProductVariantStatus.Draft;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantNameUpdatedEvent e:
                Name = e.Name;
                NormalizedName = e.NormalizedName;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantPublishedEvent e:
                Status = ProductVariantStatus.Published;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantRemovedEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                Status = ProductVariantStatus.Deleted;
                DeletedAt = e.OccurredAt;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantSizeRemovedEvent e:
                var removedSize = _sizes.SingleOrDefault(x => x.Id == e.SizeId)
                    ?? throw new DomainException(
                        "Variant size is not found.");
                
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                removedSize.Delete(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantSizeRestoredEvent e:
                var restoredSize = _sizes.SingleOrDefault(x => x.Id == e.SizeId)
                    ?? throw new DomainException(
                        "Variant size is not found.");
                
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                restoredSize.Restore(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantRestoredEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                Status = ProductVariantStatus.Draft;
                UpdatedAt = e.OccurredAt;
                DeletedAt = null;
                break;
        }
    }

    public static ProductVariant Rehydrate(
        IEnumerable<IDomainEvent> history)
    {
        var productVariant = new ProductVariant();

        foreach (var @event in history)
        {
            productVariant.Apply(@event);
            productVariant.Version++;
        }

        return productVariant;
    }
    
    private void EnsureNotDeleted(
        string? message = null)
    {
        if (Status == ProductVariantStatus.Deleted)
            throw new DomainException(
                message ?? "Entity is deleted.");
    }
    
    private void SizeEnsureNotDeleted(
        VariantSize size)
    {
        if(size.IsDeleted)
            throw new DomainException(
                "Size already was deleted.");
    }
    
    private void EnsureDetailsExists()
    {
        if (_details is null)
            throw new DomainException(
                "Details cannot be null.");
    }
    
    private void EnsureAttributeNotDeleted(
        VariantAttribute attribute)
    {
        if (attribute.IsDeleted)
            throw new DomainException(
                "Attribute already was deleted.");
    }
    
    private VariantSize GetSize(Guid sizeId)
    {
        var size = _sizes.FirstOrDefault(x => 
            x.Id == sizeId);

        if (size == null)
            throw new DomainException(
                "Size does not exist.");

        return size;
    }
    
    private VariantAttribute GetAttribute(Guid attributeId)
    {
        var attribute = _attributes.FirstOrDefault(x => 
            x.Id == attributeId);

        if (attribute == null)
            throw new DomainException(
                "Attribute does not exist.");

        return attribute;
    }
}

// TODO:
// if (variantSizeRepository.ExistsByVariantId(variantId))
// throw new DomainException("Size already exists");
// Уникальный индекс в БД (практичный)
// UNIQUE (VariantId)
// UNIQUE (VariantId, LetterSize)