using RenStore.Catalog.Domain.Aggregates.Variant.Events;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Image;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;
using RenStore.Catalog.Domain.Aggregates.Variant.Rules;
using RenStore.Catalog.Domain.Aggregates.VariantAttributes;
using RenStore.Catalog.Domain.Aggregates.VariantAttributes.Events;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Exceptions;
using RenStore.SharedKernal.Domain.ValueObjects;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

/// <summary>
/// Represents a product variant physical entity with lifecycle and invariants.
/// </summary>
public class ProductVariant
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    private readonly List<VariantAttribute> _attributes = new();
    private readonly List<ProductPriceHistory> _priceHistory = new(); // вынести в аггрегат
    private readonly List<VariantSize> _sizes = new();
    private readonly List<ProductImage> _images = new();
    
    private Color _color;
    private ProductDetail _details;
    
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
    /// Customer rating and review score for this product variant.
    /// Calculated from user reviews.
    /// </summary>
    public Rating Rating { get; private set; } 
    
    /// <summary>
    /// Internal article number that uniquely identifies this product variant.
    /// </summary>
    public long Article { get; private set; }
    
    /// <summary>
    /// Current lifecycle state on this product variant.
    /// </summary>
    public ProductVariantStatus Status { get; private set; }
    
    /// <summary>
    /// Measurement system used for sizing in this product variant.
    /// Determines which size chart applies and how sizes are displayed to customers.
    /// </summary>
    public SizeSystem SizeSystem { get; private set; }
    
    /// <summary>
    /// Category of sizing applicable to this product variant.
    /// Determines which size ranges and conversion tables are relevant.
    /// </summary>
    public SizeType SizeType { get; private set; } // нужно сделать согласование с категорией. (возможно добавить в категорию этот же энам, и при создании присваивать его)
    
    /// <summary>
    /// SEO-friendly URL slug for this product variant.
    /// Used to generate permanent links in the catalog and for search engine optimization.
    /// </summary>
    public string Url { get; private set; }
    
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
    
    /// <summary>
    /// Unique identifier of the product.
    /// </summary>
    public Guid ProductId { get; private set; }
    
    /// <summary>
    /// Unique identifier of the color.
    /// </summary>
    public int ColorId { get; private set; }
    
    /// <summary>
    /// The collection of attributes associated with this variant.
    /// </summary>
    public IReadOnlyCollection<VariantAttribute> ProductAttributes => _attributes.AsReadOnly();
    
    /// <summary>
    /// The collection of price history associated with this variant.
    /// </summary>
    public IReadOnlyCollection<ProductPriceHistory> PriceHistories => _priceHistory.AsReadOnly();
    
    /// <summary>
    /// The collection of sizes associated with this variant.
    /// </summary>
    public IReadOnlyCollection<VariantSize> Sizes => _sizes.AsReadOnly();
    
    /// <summary>
    /// The collection of images associated with this variant.
    /// </summary>
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();
    
    private ProductVariant() { }

    #region Create
    
    /// <summary>
    /// Create a new product variant in the system, linked to a specific product.
    /// </summary>
    /// <param name="now">Timestamp when the operation occurs. Used for event history.</param>
    /// <param name="productId">The unique product identifier.</param>
    /// <param name="colorId">The unique color identifier.</param>
    /// <param name="name">Display the name of this specific variant.
    /// Length must be between 25 and 500 characters after trimming.</param>
    /// <param name="inStock">Available inventory quantity for this product variant.</param>
    /// <param name="sizeSystem">Measurement system used for sizing in this product variant.
    /// Determines which size chart applies and how sizes are displayed to customers.</param>
    /// <param name="sizeType">Category of sizing applicable to this product variant.</param>
    /// <param name="url">SEO-friendly URL slug for this product variant.
    /// Used to generate permanent links in the catalog and for search engine optimization.</param>
    /// <returns>Created product variant entity with established business invariants.</returns>
    /// <exception cref="DomainException">
    /// - Invalid product or color ID
    /// - Name outside length limits (25-500 chars)
    /// - Negative stock quantity
    /// - Empty URL
    /// </exception>
    public static ProductVariant Create(
        DateTimeOffset now,
        Guid productId,
        int colorId,
        string name,
        /*int inStock,*/
        SizeSystem sizeSystem,
        SizeType sizeType,
        string url)
    {
        ProductVariantRules.ValidateProductId(productId);
        ProductVariantRules.ValidateColorId(colorId);

        var trimmedName = name.Trim();
        ProductVariantRules.ValidateName(trimmedName);

        string trimmedUrl = url.Trim();
        ProductVariantRules.ValidateUrl(trimmedUrl);
        
        var variantId = Guid.NewGuid();
        var variant = new ProductVariant();
        
        variant.Raise(
            new VariantCreated(
                VariantId: variantId,
                ProductId: productId,
                ColorId: colorId,
                Name: trimmedName,
                /*InStock: inStock,*/
                Url: trimmedUrl,
                SizeSystem: sizeSystem,
                SizeType: sizeType,
                OccurredAt: now));
        
        return variant;
    }
    
    /// <summary>
    /// Adds detailed technical and descriptive information to this product variant.
    /// Includes manufacturing details, materials, care instructions, and product features.
    /// </summary>
    /// <param name="now">Timestamp for creation audit</param>
    /// <param name="countryOfManufactureId">Country where the product was manufactured</param>
    /// <param name="description">Full product description for customers</param>
    /// <param name="modelFeatures">Key features and specifications of this model</param>
    /// <param name="decorativeElements">Decorative or design elements</param>
    /// <param name="equipment">Included accessories and packaging contents</param>
    /// <param name="composition">Material composition and percentages</param>
    /// <param name="caringOfThings">Care and maintenance instructions</param>
    /// <param name="typeOfPackaging">Type of packaging (optional)</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Variant is deleted
    /// - Details already exist for this variant
    /// - Required fields fail validation
    /// - Country ID is invalid
    /// </exception>
    /// <remarks>
    /// Details can only be added once per variant. Use UpdateDetails to modify existing information.
    /// </remarks>
    public void AddDetails(
        DateTimeOffset now,
        int countryOfManufactureId,
        string description,
        string modelFeatures,
        string decorativeElements,
        string equipment,
        string composition,
        string caringOfThings,
        TypeOfPackaging? typeOfPackaging = null)
    {
        EnsureNotDeleted();
        
        if (_details != null)
            throw new DomainException("Product details already was created!");

        ProductDetailRules.CountryOfManufactureValidate(countryOfManufactureId);
        ProductDetailRules.ProductVariantIdValidate(Id);

        var trimmedDescription              = ProductDetailRules.DescriptionNormalizedAndValidate(description);
        var trimmedComposition        = ProductDetailRules.CompositionNormalizedAndValidate(composition);
        var trimmedModelFeatures      = ProductDetailRules.ModelFeaturesNormalizedAndValidate(modelFeatures);
        var trimmedDecorativeElements = ProductDetailRules.DecorativeElementsNormalizedAndValidate(decorativeElements);
        var trimmedEquipment          = ProductDetailRules.EquipmentNormalizedAndValidate(equipment);
        var trimmedCaringOfThings     = ProductDetailRules.CaringOfThingsNormalizedAndValidate(caringOfThings);

        var detailId = Guid.NewGuid();
        
        Raise(new VariantDetailsCreated(
            OccurredAt: now,
            Id: detailId,
            VariantId: Id,
            CountryOfManufactureId: countryOfManufactureId,
            ModelFeatures: trimmedModelFeatures,
            DecorativeElements: trimmedDecorativeElements,
            Equipment: trimmedEquipment,
            Description: trimmedDescription,
            Composition: trimmedComposition,
            CaringOfThings: trimmedCaringOfThings,
            TypeOfPackaging: typeOfPackaging ?? null));
    }

    /// <summary>
    /// Adds a new size option with inventory to this product variant.
    /// Each variant can have multiple size options with independent stock levels.
    /// </summary>
    /// <param name="letterSize">Alphanumeric size designation (e.g., "M", "10", "42")</param>
    /// <param name="inStock">Available quantity for this specific size</param>
    /// <param name="now">Timestamp for creation audit</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Variant ID is invalid
    /// - Stock quantity is negative
    /// - Size already exists for this variant
    /// - Size is incompatible with variant's SizeType/SizeSystem
    /// </exception>
    /// <remarks>
    /// Size validation ensures compatibility with the variant's SizeType (Clothes/Shoes)
    /// and SizeSystem (RU/US/EU). Each size maintains its own stock level.
    /// </remarks>
    public void AddSize(
        LetterSize letterSize,
        int inStock,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        VariantSizeRules.InStockValidate(inStock);

        if (_sizes.Any(x => x.Size.LetterSize == letterSize))
            throw new DomainException("The size already exits in the system.");

        Size.Validate(
            size: letterSize, 
            type: SizeType, 
            system: SizeSystem);
        
        var variantSizeId = Guid.NewGuid();
        
        Raise(new VariantSizeCreated(
            OccurredAt: now,
            VariantSizeId: variantSizeId,
            VariantId: Id,
            InStock: inStock,
            LetterSize: letterSize,
            SizeSystem: SizeSystem,
            SizeType: SizeType));
    }
    
    /// <summary>
    /// Adds a descriptive attribute to this product variant.
    /// Attributes represent specific features or specifications (e.g., "Material: Cotton", "Screen Size: 6.1").
    /// </summary>
    /// <param name="now">Timestamp for creation audit</param>
    /// <param name="key">Attribute category or name (e.g., "Material", "Weight")</param>
    /// <param name="value">Attribute specification (e.g., "Leather", "1.5kg")</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Variant is deleted
    /// - Maximum attribute limit reached
    /// - Key or value fails validation (empty, too long, etc.)
    /// - Variant ID is invalid
    /// </exception>
    /// <remarks>
    /// Attributes help customers filter and compare products. 
    /// Each variant can have multiple attributes describing its unique characteristics.
    /// </remarks>
    public void AddAttribute(
        DateTimeOffset now,
        string key,
        string value)
    {
        EnsureNotDeleted();

        ProductVariantRules.MaxAttributesCountValidation(_attributes.Count);
        ProductAttributeRules.ProductVariantIdNormalizeAndValidate(Id);
        
        var trimmedKey   = ProductAttributeRules.KeyNormalizeAndValidate(key);
        var trimmedValue = ProductAttributeRules.ValueNormalizeAndValidate(value);
        
        Raise(new VariantAttributeCreated(
            VariantId: Id,
            OccurredAt: now,
            Key: trimmedKey,
            Value: trimmedValue));
    }
    
    public void AddImage(
        DateTimeOffset now,
        Guid imageId,
        string originalFileName,
        string storagePath,
        long fileSizeBytes,
        bool isMain,
        short sortOrder,
        int weight, 
        int height)
    {
        ProductImageRules.CreateProductImageValidation(
            imageId: imageId,
            productVariantId: Id,
            originalFileName: originalFileName,
            storagePath: storagePath,
            fileSizeBytes: fileSizeBytes,
            sortOrder: sortOrder,
            weight: weight,
            height: height);
        
        if (isMain)
        {
            var currentMain = _images.FirstOrDefault(x => x.IsMain);

            if (currentMain != null)
            {
                Raise(new VariantImageMainUnset(
                    OccurredAt: now,
                    VariantId: Id,
                    ImageId: currentMain.Id));
            }
        }
        
        Raise(new VariantImageCreated(
            OccurredAt: now,
            ImageId: imageId,
            VariantId: Id,
            OriginalFileName: originalFileName,
            StoragePath: storagePath,
            FileSizeBytes: fileSizeBytes,
            IsMain: isMain,
            SortOrder: sortOrder,
            Weight: weight,
            Height: height));
    }
    
    #endregion

    #region Update
    
    // TODO:
    public void ChangeColor()
    {
        EnsureNotDeleted();
    }
    
    /// <summary>
    /// Updates the display name of this product variant.
    /// The normalized version (uppercase) is automatically regenerated for search consistency.
    /// </summary>
    /// <param name="now">Timestamp for the update audit</param>
    /// <param name="name">New customer-facing variant name</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Variant is deleted
    /// - Name fails validation (length 25-500 characters)
    /// </exception>
    /// <remarks>
    /// If the new name is identical to the current name (after trimming), no changes are made.
    /// The change is idempotent and only triggers events when actual modification occurs.
    /// </remarks>
    public void ChangeName(
        DateTimeOffset now,
        string name)
    {
        EnsureNotDeleted();
        
        string trimmedName = name.Trim();
        
        if (trimmedName == Name) return;
        
        ProductVariantRules.ValidateName(trimmedName);

        Raise(new VariantNameUpdated(
            OccurredAt: now,
            VariantId: Id,
            Name: trimmedName));
    }
    
    /// <summary>
    /// Updates the average customer rating for this product variant.
    /// Calculated from customer reviews and feedback scores.
    /// </summary>
    /// <param name="score">New average rating score</param>
    /// <param name="now">Timestamp for rating update</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Variant is deleted
    /// - Score is outside valid rating range (handled by validation rules)
    /// </exception>
    /// <remarks>
    /// Ratings typically range from 0.0 to 5.0 or similar scale.
    /// This method accepts pre-calculated averages; individual review processing
    /// should be handled separately in a review aggregation service.
    /// </remarks>
    public void UpdateRating(
        decimal score,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        Rating.AddRatingValidate(score);
        
        Raise(new VariantAverageRatingUpdated(
            VariantId: Id,
            OccurredAt: now,
            Score: score));
    }

    public void Activate(DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if (Status == ProductVariantStatus.Published)
            return;
        
        Raise(new VariantPublished(
            OccurredAt: now,
            VariantId: Id));
    }
    
    public void Archive(DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if (Status == ProductVariantStatus.Archived)
            return;
        
        Raise(new VariantArchived(
            OccurredAt: now,
            VariantId: Id));
    }

    public void ChangeDetailDescription(
        DateTimeOffset now,
        string description)
    {
        EnsureNotDeleted();
        EnsureNotEmptyDetails();
        
        var trimmedDescription = ProductDetailRules.DescriptionNormalizedAndValidate(description);
        
        if(_details.Description == trimmedDescription) return;
        
        Raise(new VariantDetailsDescriptionUpdated(
            OccurredAt: now,
            VariantId: Id,
            Description: trimmedDescription));
    }

    public void ChangeDetailModelFeatures(
        DateTimeOffset now,
        string modelFeatures)
    {
        EnsureNotDeleted();
        EnsureNotEmptyDetails();
        
        var trimmedModelFeatures = ProductDetailRules.ModelFeaturesNormalizedAndValidate(modelFeatures);
        
        if(_details.ModelFeatures == trimmedModelFeatures) return;
        
        Raise(new VariantDetailsModelFeaturesUpdated(
            OccurredAt: now,
            VariantId: Id,
            ModelFeatures: trimmedModelFeatures));
    }

    public void ChangeDetailDecorativeElements(
        DateTimeOffset now,
        string decorativeElements)
    {
        EnsureNotDeleted();
        EnsureNotEmptyDetails();
        
        var trimmedDecorativeElements = ProductDetailRules.DecorativeElementsNormalizedAndValidate(decorativeElements);
        
        if(string.IsNullOrEmpty(trimmedDecorativeElements))
            throw new DomainException("Product Detail decorative elements cannot be null or whitespace.");
        
        if(_details.DecorativeElements == trimmedDecorativeElements) return;
        
        Raise(new VariantDetailsDecorativeElementsUpdated(
            OccurredAt: now,
            VariantId: Id,
            DecorativeElements: trimmedDecorativeElements));
    }

    public void ChangeDetailEquipment(
        DateTimeOffset now,
        string equipment)
    {
        EnsureNotDeleted();
        EnsureNotEmptyDetails();
        
        var trimmedEquipment = ProductDetailRules.EquipmentNormalizedAndValidate(equipment);
        
        if (string.IsNullOrEmpty(trimmedEquipment))
            throw new DomainException("Product Detail equipment cannot be null or whitespace.");
        
        if(_details.Equipment == trimmedEquipment) return;
        
        Raise(new VariantDetailsEquipmentUpdated(
            OccurredAt: now,
            VariantId: Id,
            Equipment: trimmedEquipment));
    }

    public void ChangeDetailComposition(
        DateTimeOffset now,
        string composition)
    {
        EnsureNotDeleted();
        EnsureNotEmptyDetails();
        
        var trimmedComposition = ProductDetailRules.CompositionNormalizedAndValidate(composition);
        
        if (string.IsNullOrEmpty(composition))
            throw new DomainException("Product Detail composition cannot be null or whitespace.");
        
        if(_details.Composition == trimmedComposition) return;
        
        Raise(new VariantDetailsCompositionUpdated(
            OccurredAt: now,
            VariantId: Id,
            Composition: trimmedComposition));
    }

    public void ChangeDetailCaringOfThings(
        DateTimeOffset now,
        string caringOfThings)
    {
        EnsureNotDeleted();
        EnsureNotEmptyDetails();
        
        var trimmedCaringOfThings = ProductDetailRules.CaringOfThingsNormalizedAndValidate(caringOfThings);
        
        if (string.IsNullOrEmpty(caringOfThings))
            throw new DomainException("Product Detail Caring Of Things cannot be null or whitespace.");
        
        if(_details.CaringOfThings == trimmedCaringOfThings) return;
        
        Raise(new VariantDetailsCaringOfThingsUpdated(
            OccurredAt: now,
            VariantId: Id,
            CaringOfThings: trimmedCaringOfThings));
    }

    public void ChangeDetailTypeOfPacking(
        DateTimeOffset now,
        TypeOfPackaging typeOfPackaging)
    {
        EnsureNotDeleted();
        EnsureNotEmptyDetails();
        
        Raise(new VariantDetailsTypeOfPackingUpdated(
            OccurredAt: now,
            VariantId: Id,
            TypeOfPackaging: typeOfPackaging));
    }

    public void ChangeCountryOfManufactureId(
        DateTimeOffset now,
        int countryOfManufactureId)
    {
        EnsureNotDeleted();
        EnsureNotEmptyDetails();
        
        if(countryOfManufactureId <= 0)
            throw new DomainException("Product Detail country of manufacture ID must be more then 0.");
        
        if(_details.CountryOfManufactureId == countryOfManufactureId) 
            return;
        
        Raise(new VariantDetailsCountryOfManufactureIdUpdated(
            OccurredAt: now,
            VariantId: Id,
            CountryOfManufactureId: countryOfManufactureId));
    }

    public void ChangeAttributeKey(
        DateTimeOffset now,
        Guid attributeId,
        string key)
    {
        EnsureNotDeleted();
        
        var attribute = GetAttribute(attributeId);
        AttributeEnsureNotDeleted(attribute);
        
        var trimmedKey = ProductAttributeRules
            .KeyNormalizeAndValidate(key);
        
        if(attribute.Key == trimmedKey) return;
        
        Raise(new VariantAttributeKeyUpdated(
            OccurredAt: now,
            VariantId: Id,
            AttributeId: attributeId,
            Key: trimmedKey));
    }
    
    public void ChangeAttributeValue(
        DateTimeOffset now,
        Guid attributeId,
        string value)
    {
        EnsureNotDeleted();
        
        var attribute = GetAttribute(attributeId);
        AttributeEnsureNotDeleted(attribute);
        
        var trimmedValue = ProductAttributeRules
            .ValueNormalizeAndValidate(value);
        
        if(attribute.Value == trimmedValue) return;
        
        Raise(new VariantAttributeValueUpdated(
            OccurredAt: now,
            VariantId: Id,
            AttributeId: attributeId,
            Value: trimmedValue));
    }
    
    public void MarkImageAsMain(
        DateTimeOffset now,
        Guid imageId)
    {
        EnsureNotDeleted();
        
        var currentMain = _images.FirstOrDefault(x => x.IsMain);
        
        if(currentMain?.Id == imageId)
            return;

        if (currentMain != null)
        {
            Raise(new VariantImageMainUnset(
                OccurredAt: now,
                VariantId: Id,
                ImageId: currentMain.Id));
        }
        
        Raise(new VariantImageMainSet(
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));
    }

    public void ChangeImageSortOrder(
        DateTimeOffset now,
        Guid imageId,
        short sortOrder)
    {
        EnsureNotDeleted();
        
        var image = GetImage(imageId);
        
        if(image.SortOrder == sortOrder) 
            return;
        
        Raise(new VariantImageSortOrderUpdated(
            OccurredAt: now,
            ImageId: imageId,
            SortOrder: sortOrder));
    }

    public void ChangeImageStoragePath(
        DateTimeOffset now,
        Guid imageId,
        string storagePath)
    {
        EnsureNotDeleted();
        
        ProductImageRules.StoragePathValidate(storagePath);

        var image = GetImage(imageId);
        ImageEnsureNotDeleted(image);
        
        if(image.StoragePath == storagePath) 
            return;
        
        Raise(new VariantImageStoragePathUpdated(
            OccurredAt: now,
            ImageId: imageId,
            StoragePath: storagePath));
    }

    public void ChangeImageDimension(
        DateTimeOffset now,
        Guid imageId,
        int weight,
        int height)
    {
        EnsureNotDeleted();
        
        ProductImageRules.WeightAndHeightValidate(
            weight: weight, 
            height: height);
        
        var image = GetImage(imageId);
        ImageEnsureNotDeleted(image);
        
        if(image.Weight == weight && image.Height == height) 
            return;
        
        Raise(new VariantImageDimensionChanged(
            OccurredAt: now,
            ImageId: imageId,
            Weight: weight,
            Height: height));
    }

    public void ChangeImageFileSizeBytes(
        Guid imageId,
        DateTimeOffset now,
        long fileSizeBytes)
    {
        EnsureNotDeleted();
        
        var image = GetImage(imageId);
        ImageEnsureNotDeleted(image);
            
        Raise(new VariantImageFileSizeBytesChanged(
            OccurredAt: now,
            ImageId: imageId,
            FileSizeBytes: fileSizeBytes));
    }

    public void SetSizeStock(
        DateTimeOffset now,
        Guid sizeId,
        int newStock)
    {
        EnsureNotDeleted();

        var size = GetSize(sizeId);
        SizeEnsureNotDeleted(size);
        
        VariantSizeRules.InStockValidate(newStock);
        
        if(size.InStock == newStock) return;
        
        Raise(new VariantSizeStockSetted(
            OccurredAt: now,
            VariantId: Id,
            VariantSizeId: sizeId,
            NewStock: newStock));
    }
    
    public void AddToSizeStock(
        DateTimeOffset now,
        Guid sizeId,
        int count)
    {
        EnsureNotDeleted();

        var size = GetSize(sizeId);
        SizeEnsureNotDeleted(size);
        
        VariantSizeRules.ChangeCountValidate(count);

        var newStock = size.InStock + count;
        
        VariantSizeRules.InStockValidate(newStock);
        
        Raise(new VariantSizeStockAdded(
            OccurredAt: now,
            VariantId: Id,
            VariantSizeId: sizeId,
            Count: count));
    }

    public void RemoveFromSizeStock(
        DateTimeOffset now,
        Guid sizeId,
        int count)
    {
        EnsureNotDeleted();

        var size = GetSize(sizeId);
        SizeEnsureNotDeleted(size);
        
        VariantSizeRules.ChangeCountValidate(count);
        
        if(count > size.InStock)
            throw new DomainException("The count of sells exceed available count.");
        
        Raise(new VariantSizeRemovedFromStock(
            OccurredAt: now,
            VariantId: Id,
            VariantSizeId: sizeId,
            Count: count));
    }
    
    #endregion
    
    #region Update / Restore
    public void DeleteImage(
        DateTimeOffset now,
        Guid imageId)
    {
        var image = GetImage(imageId);
        ImageEnsureNotDeleted(image);
        
        Raise(new VariantImageRemoved(
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));
    }

    public void RestoreImage(
        DateTimeOffset now,
        Guid imageId)
    {
        EnsureNotDeleted();
        
        var image = GetImage(imageId);
        
        if(!image.IsDeleted)
            throw new DomainException("Image was not deleted.");
        
        Raise(new VariantImageRestored(
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));
    }
    
    public void Delete(DateTimeOffset now)
    {
        if (Status == ProductVariantStatus.IsDeleted)
            throw new DomainException("Cannot delete already deleted variant.");
        
        Raise(new ProductVariantRemoved(
            VariantId: Id,
            OccurredAt: now));
    }
    
    /// <summary>
    /// Removes a specific attribute from this product variant.
    /// Used when attribute information is no longer relevant or correct.
    /// </summary>
    /// <param name="now">Timestamp for deletion audit</param>
    /// <param name="attributeId">Identifier of the attribute to remove</param>
    /// <exception cref="DomainException">
    /// Thrown when attribute with specified ID is not found in this variant
    /// </exception>
    /// <remarks>
    /// This is a logical deletion that removes the attribute from display,
    /// but may preserve it for audit purposes in the underlying storage.
    /// The attribute must exist and belong to this variant to be deleted.
    /// </remarks>
    public void DeleteAttribute(
        DateTimeOffset now,
        Guid attributeId)
    {
        EnsureNotDeleted();
        
        var existingAttribute = _attributes
            .FirstOrDefault(x => x.Id == attributeId);

        if (existingAttribute == null)
            throw new DomainException("Cannot delete not exists attribute.");
        
        
        Raise(new VariantAttributeRemoved(
            OccurredAt: now,
            VariantId: Id,
            AttributeId: attributeId));
    }
    
    /// <summary>
    /// Removes a specific size option from this product variant's available selections.
    /// Used when a size is discontinued, out of stock permanently, or no longer manufactured.
    /// </summary>
    /// <param name="now">Timestamp for deletion audit</param>
    /// <param name="variantSizeId">Identifier of the size option to remove</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Size option with specified ID is not found
    /// - Size option is already marked as deleted
    /// </exception>
    /// <remarks>
    /// Size deletion affects customers who may have previously purchased this size.
    /// Consider alternatives like marking as "out of stock" instead of deletion for better UX.
    /// Removal is logical (soft delete) for historical order reference.
    /// </remarks>
    public void DeleteSize(
        DateTimeOffset now,
        Guid variantSizeId)
    {
        EnsureNotDeleted();
        
        var existingSize = _sizes
            .FirstOrDefault(x => x.Id == variantSizeId);

        if (existingSize == null)
            throw new DomainException("The letterSize is not found.");
        
        if(existingSize.IsDeleted)
            throw new DomainException("Cannot dele already deleted letterSize.");
        
        Raise(new VariantSizeRemoved(
            OccurredAt: now,
            VariantId: Id,
            VariantSizeId: variantSizeId));
    }
    
    /// <summary>
    /// Restores a previously deleted attribute to this product variant.
    /// Reverses the effect of <see cref="DeleteAttribute"/> while preserving the attribute's history.
    /// </summary>
    /// <param name="now">Timestamp for restoration audit</param>
    /// <param name="attributeId">Identifier of the attribute to restore</param>
    /// <exception cref="DomainException">
    /// Thrown when attribute with specified ID is not found in this variant
    /// </exception>
    /// <remarks>
    /// Restoration makes the attribute visible again in product listings and specifications.
    /// The attribute must have been previously deleted to be eligible for restoration.
    /// Restoration preserves the attribute's original data and relationships.
    /// </remarks>
    public void RestoreAttribute(
        DateTimeOffset now,
        Guid attributeId)
    {
        EnsureNotDeleted();
        
        var existingAttribute = _attributes
            .FirstOrDefault(x => x.Id == attributeId);

        if (existingAttribute == null)
            throw new DomainException("Cannot delete not exists attribute.");
        
        Raise(new VariantAttributeRestored(
            OccurredAt: now,
            VariantId: Id,
            AttributeId: attributeId));
    }

    /// <summary>
    /// Restores a previously deleted size option, making it available for purchase again.
    /// Reverses the effect of <see cref="DeleteSize"/> while preserving the size's configuration.
    /// </summary>
    /// <param name="now">Timestamp for restoration audit</param>
    /// <param name="variantSizeId">Identifier of the size option to restore</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Size option with specified ID is not found
    /// - Size option is not currently marked as deleted
    /// </exception>
    /// <remarks>
    /// Restored sizes regain their previous stock levels and availability status.
    /// Customers can once again select this size when purchasing the variant.
    /// Consider verifying manufacturer availability before restoring discontinued sizes.
    /// </remarks>
    public void RestoreSize(
        DateTimeOffset now,
        Guid variantSizeId)
    {
        EnsureNotDeleted();
        
        var existingSize = _sizes.FirstOrDefault(x => x.Id == variantSizeId);

        if (existingSize == null)
            throw new DomainException("Product size not exists.");
        
        if (!existingSize.IsDeleted)
            throw new DomainException("The size was not deleted.");
        
        Raise(new VariantSizeRestored(
            OccurredAt: now,
            VariantId: Id,
            VariantSizeId: variantSizeId));
    }
    
    #endregion
    
    protected override void Apply(object @event)
    {
        switch (@event)
        {
            case VariantCreated e:
                CreatedAt = e.OccurredAt;
                Id = e.VariantId;
                ProductId = e.ProductId;
                ColorId = e.ColorId;
                Name = e.Name;
                NormalizedName = e.Name.ToUpperInvariant();
                /*_stock = VariantStock.Create(e.InStock);*/ // TODO:
                Url = e.Url;
                SizeSystem = e.SizeSystem;
                SizeType = e.SizeType;
                Status = ProductVariantStatus.Draft;
                Rating = Rating.Empty();
                break;
            
            case VariantDetailsCreated e:
                _details = ProductDetail.Create(
                    id: e.Id,
                    now: e.OccurredAt,
                    countryOfManufactureId: e.CountryOfManufactureId,
                    productVariantId: e.VariantId,
                    description: e.Description,
                    composition: e.Composition,
                    caringOfThings: e.CaringOfThings,
                    typeOfPackaging: e.TypeOfPackaging, 
                    modelFeatures: e.ModelFeatures, 
                    decorativeElements: e.DecorativeElements, 
                    equipment: e.Equipment);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantAttributeCreated e:
                _attributes.Add(VariantAttribute.Create(
                    key: e.Key,
                    value: e.Value,
                    productVariantId: e.VariantId,
                    now: e.OccurredAt));
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantSizeCreated e:
                _sizes.Add(VariantSize.Create(
                    now: e.OccurredAt,
                    size: Size.Create(
                        size: e.LetterSize, 
                        type: e.SizeType, 
                        system: e.SizeSystem),
                    inStock: e.InStock,
                    variantId: e.VariantId));
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantPublished e:
                Status = ProductVariantStatus.Published;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantArchived e:
                Status = ProductVariantStatus.Archived;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantNameUpdated e:
                Name = e.Name;
                NormalizedName = e.Name.ToUpperInvariant();
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantAttributeKeyUpdated e:
                _attributes.Single(x => x.Id == e.AttributeId)
                    .ChangeKey(
                        now: e.OccurredAt,
                        key: e.Key);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantAttributeValueUpdated e:
                _attributes.Single(x => x.Id == e.AttributeId)
                    .ChangeValue(
                        now: e.OccurredAt,
                        value: e.Value);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantAverageRatingUpdated e:
                Rating = Rating.Add(e.Score);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageCreated e:
                _images.Add(ProductImage.Create(
                    now: e.OccurredAt,
                    imageId: e.ImageId,
                    variantId: e.VariantId,
                    originalFileName: e.OriginalFileName,
                    storagePath: e.StoragePath,
                    fileSizeBytes: e.FileSizeBytes,
                    isMain: e.IsMain,
                    sortOrder: e.SortOrder,
                    weight: e.Weight,
                    height: e.Height));
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageSortOrderUpdated e:
                _images.Single(x => x.Id == e.ImageId)
                    .ChangeSortOrder(
                        now: e.OccurredAt,
                        sortOrder: e.SortOrder);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageStoragePathUpdated e:
                _images.Single(x => x.Id == e.ImageId)
                    .ChangeStoragePath(
                        now: e.OccurredAt,
                        storagePath: e.StoragePath);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageDimensionChanged e:
                _images.Single(x => x.Id == e.ImageId)
                    .ChangeDimension(
                        now: e.OccurredAt,
                        weight: e.Weight,
                        height: e.Height);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageFileSizeBytesChanged e:
                _images.Single(x => x.Id == e.ImageId)
                    .ChangeFileSizeBytes(
                        now: e.OccurredAt,
                        fileSizeBytes: e.FileSizeBytes);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageMainUnset e:
                _images.Single(x => x.Id == e.ImageId)
                    .UnsetAsMain(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageMainSet e:
                _images.Single(x => x.Id == e.ImageId)
                    .SetAsMain(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsCaringOfThingsUpdated e:
                _details.ChangeCaringOfThings(
                    now: e.OccurredAt,
                    caringOfThings: e.CaringOfThings);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsCompositionUpdated e:
                _details.ChangeComposition(
                    now: e.OccurredAt,
                    composition: e.Composition);
                    UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsCountryOfManufactureIdUpdated e:
                _details.ChangeCountryOfManufactureId(
                    now: e.OccurredAt,
                    countryOfManufactureId: e.CountryOfManufactureId);
                    UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsDecorativeElementsUpdated e:
                _details.ChangeDecorativeElements(
                    now: e.OccurredAt,
                    decorativeElements: e.DecorativeElements);
                    UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsDescriptionUpdated e:
                _details.ChangeDescription(
                    now: e.OccurredAt,
                    description: e.Description);
                    UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsEquipmentUpdated e:
                _details.ChangeEquipment(
                    now: e.OccurredAt,
                    equipment: e.Equipment);
                    UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsModelFeaturesUpdated e:
                _details.ChangeModelFeatures(
                    now: e.OccurredAt,
                    modelFeatures: e.ModelFeatures);
                    UpdatedAt = e.OccurredAt;
                break;
            
            case VariantDetailsTypeOfPackingUpdated e:
                _details.ChangeTypeOfPacking(
                    now: e.OccurredAt,
                    typeOfPackaging: e.TypeOfPackaging);
                    UpdatedAt = e.OccurredAt;
                break;
            
            case VariantSizeStockSetted e:
                _sizes.Single(x => x.Id == e.VariantSizeId)
                    .SetStock(
                        now: e.OccurredAt,
                        newStock: e.NewStock);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantSizeStockAdded e:
                _sizes.Single(x => x.Id == e.VariantSizeId)
                    .AddToStock(
                        now: e.OccurredAt,
                        count: e.Count);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantSizeRemovedFromStock e:
                _sizes.Single(x => x.Id == e.VariantSizeId)
                    .RemoveFromStock(
                        now: e.OccurredAt,
                        count: e.Count);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageRemoved e:
                _images.Single(x => x.Id == e.ImageId)
                    .Delete(now: e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductVariantRemoved e:
                Status = ProductVariantStatus.IsDeleted;
                DeletedAt = e.OccurredAt;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantAttributeRemoved e:
                _attributes
                    .Single(x => x.Id == e.AttributeId)
                    .Delete(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantSizeRemoved e:
                _sizes
                    .Single(x => x.Id == e.VariantSizeId)
                    .Delete(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantAttributeRestored e:
                _attributes
                    .Single(x => x.Id == e.AttributeId)
                    .Restore(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantSizeRestored e:
                _sizes
                    .Single(x => x.Id == e.VariantSizeId)
                    .Restore(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageRestored e:
                _images.Single(x => x.Id == e.ImageId)
                    .Restore(now: e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
        }
    }
    
    /// <summary>
    /// Ensures the product variant is not deleted before performing operations.
    /// </summary>
    /// <param name="message">Optional custom error message</param>
    /// <exception cref="DomainException">Thrown when entity is deleted</exception>
    private void EnsureNotDeleted(string? message = null)
    {
        if (Status == ProductVariantStatus.IsDeleted)
            throw new DomainException(message ?? "Entity is deleted.");
    }
    
    /// <summary>
    /// Ensures the variant image is not deleted before performing operations.
    /// </summary>
    /// <param name="image">The variant image to check.</param>
    /// <exception cref="DomainException">Thrown when image is deleted</exception>
    private void ImageEnsureNotDeleted(ProductImage image)
    {
        if(image.IsDeleted)
            throw new DomainException("Image already was deleted.");
    }
    
    /// <summary>
    /// Ensures the variant size is not deleted before performing operations.
    /// </summary>
    /// <param name="size">The variant size to check.</param>
    /// <exception cref="DomainException">Thrown when size is deleted</exception>
    private void SizeEnsureNotDeleted(VariantSize size)
    {
        if(size.IsDeleted)
            throw new DomainException("Size already was deleted.");
    }
    
    /// <summary>
    /// Ensures the variant attribute is not deleted before performing operations.
    /// </summary>
    /// <param name="attribute">The variant attribute to check.</param>
    /// <exception cref="DomainException">Thrown when attribute is deleted</exception>
    private void AttributeEnsureNotDeleted(VariantAttribute attribute)
    {
        if(attribute.IsDeleted)
            throw new DomainException("Attribute already was deleted.");
    }
    
    /// <summary>
    /// Ensures the variant details is not empty before performing operations.
    /// </summary>
    /// <exception cref="DomainException">Thrown when details is deleted</exception>
    private void EnsureNotEmptyDetails()
    {
        if (_details == null)
            throw new DomainException("Cannot change empty details");
    }
    
    private ProductImage GetImage(Guid imageId)
    {
        var image = _images.FirstOrDefault(x => x.Id == imageId);

        if (image == null)
            throw new DomainException("Image does not exist.");

        return image;
    }
    
    private VariantAttribute GetAttribute(Guid attributeId)
    {
        var attribute = _attributes.FirstOrDefault(x => x.Id == attributeId);

        if (attribute == null)
            throw new DomainException("Attribute does not exist.");

        return attribute;
    }
    
    private VariantSize GetSize(Guid sizeId)
    {
        var size = _sizes.FirstOrDefault(x => x.Id == sizeId);

        if (size == null)
            throw new DomainException("Size does not exist.");

        return size;
    }
}

// TODO: if (Status != ProductVariantStatus.Published) - переносится в Application Layer
//           throw new DomainException("Variant is not published."); 

// TODO: сделать создание
//  сделать методы: Activate() Archive()
//  «нельзя удалить main без переназначения»
//  1) цвета, истории цен,
//  2) сделат методы для изменеия SizeType, SizeSystem (тобиж изменение таблицы размеров)
//  - ⚠️ Это очень опасная операция:
//  - размеры уже существуют
//  - размеры привязаны к старой таблице
//  - Правильно:
//  - либо запретить изменение
//  - либо делать mass migration event (VariantSizeSystemChanged) с пересозданием размеров
//  добавить isAvailable в продажи и тд