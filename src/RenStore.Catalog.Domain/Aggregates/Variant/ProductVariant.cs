using RenStore.Catalog.Domain.Aggregates.Variant.Events;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Attribute;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;
using RenStore.Catalog.Domain.Aggregates.Variant.Rules;
using RenStore.Catalog.Domain.Aggregates.VariantDetails;
using RenStore.Catalog.Domain.Aggregates.VariantSizes.Events;
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
    
    private readonly List<ProductPriceHistory> _priceHistory = new(); // вынести в аггрегат
    private readonly List<VariantSize> _sizes = new();
    private readonly List<Guid> _imageIds = new();
    private readonly List<Guid> _attributeIds = new();
    
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
    /// SEO-friendly URL slug for this product variant.
    /// Used to generate permanent links in the catalog and for search engine optimization.
    /// </summary>
    public string Url { get; private set; }
    
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
    /// The collection of price history associated with this variant.
    /// </summary>
    public IReadOnlyCollection<ProductPriceHistory> PriceHistories => _priceHistory.AsReadOnly();
    
    /// <summary>
    /// The collection of sizes associated with this variant.
    /// </summary>
    public IReadOnlyCollection<VariantSize> Sizes => _sizes.AsReadOnly();
    
    /// <summary>
    /// The collection of images IDs associated with this variant.
    /// </summary>
    public IReadOnlyCollection<Guid> ImageIds => _imageIds.AsReadOnly();
    
    /// <summary>
    /// The collection of attributes IDs associated with this variant.
    /// </summary>
    public IReadOnlyCollection<Guid> AttributeIds => _attributeIds.AsReadOnly();
    
    private ProductVariant() { }

    #region Variant
    
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
    
    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted("Cannot delete already deleted variant.");
        
        Raise(new ProductVariantRemoved(
            VariantId: Id,
            OccurredAt: now));
    }
    
    #endregion

    #region Details

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
    
    #endregion

    #region size

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

    #region Attribute

    public void AddAttributeId(
        DateTimeOffset now,
        Guid attributeId)
    {
        EnsureNotDeleted();
        
        ProductVariantRules.MaxAttributesCountValidation(_attributeIds.Count + 1);
        
        if (_attributeIds.Contains(attributeId))
            throw new DomainException("Attribute already exists.");
        
        Raise(new VariantAttributeAdded(
            OccurredAt: now,
            VariantId: Id,
            AttributeId: attributeId));
    }
    
    public void RemoveAttributeId(
        DateTimeOffset now,
        Guid attributeId)
    {
        EnsureNotDeleted();

        if (!_attributeIds.Contains(attributeId))
            throw new DomainException("Attribute does not exist.");
        
        Raise(new VariantAttributeRemoved(
            OccurredAt: now,
            VariantId: Id,
            AttributeId: attributeId));
    }

    #endregion

    #region Image

    public void AddImageId(
        DateTimeOffset now,
        Guid imageId)
    {
        EnsureNotDeleted();
        
        ProductVariantRules.MaxImagesCountValidation(_imageIds.Count + 1);

        if (_imageIds.Contains(imageId))
            throw new DomainException("Image already exists.");
        
        Raise(new VariantImageAdded(
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));
    }
    
    public void MarkImageAsMain(
        DateTimeOffset now,
        Guid imageId)
    {
        /*EnsureNotDeleted();

        var currentMain = _imageIds.FirstOrDefault(x => x.IsMain);

        if(currentMain?.Id == imageId)
            return;

        if (currentMain != null)
        {
            Raise(new ImageMainUnset(
                OccurredAt: now,
                VariantId: Id,
                ImageId: currentMain.Id));
        }

        Raise(new ImageMainSet(
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));*/
    }
    
    public void RemoveImageId(
        DateTimeOffset now,
        Guid imageId)
    {
        EnsureNotDeleted();

        if (!_imageIds.Contains(imageId))
            throw new DomainException("Image does not exist.");
        
        Raise(new VariantImageRemoved(
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));
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
            
            case VariantSizeCreated e:
                _sizes.Add(VariantSize.Create(
                    id: e.VariantSizeId,
                    now: e.OccurredAt,
                    size: Size.Create(
                        size: e.LetterSize, 
                        type: e.SizeType, 
                        system: e.SizeSystem),
                    inStock: e.InStock,
                    variantId: e.VariantId));
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantAttributeAdded e:
                _attributeIds.Add(e.AttributeId);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageAdded e:
                _imageIds.Add(e.ImageId);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantAttributeRemoved e:
                _attributeIds.Remove(e.AttributeId);
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
            
            case VariantAverageRatingUpdated e:
                Rating = Rating.Add(e.Score);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageMainSet e:
                // TODO:
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
                _imageIds.Remove(e.ImageId);
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductVariantRemoved e:
                Status = ProductVariantStatus.IsDeleted;
                DeletedAt = e.OccurredAt;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantSizeRemoved e:
                _sizes
                    .Single(x => x.Id == e.VariantSizeId)
                    .Delete(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantSizeRestored e:
                _sizes
                    .Single(x => x.Id == e.VariantSizeId)
                    .Restore(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
        }
    }
    
    /// <summary>
    /// Ensures the product variant is not deleted before performing operations.
    /// </summary>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="DomainException">Thrown when entity is deleted.</exception>
    private void EnsureNotDeleted(string? message = null)
    {
        if (Status == ProductVariantStatus.IsDeleted)
            throw new DomainException(message ?? "Entity is deleted.");
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
    
    private VariantSize GetSize(Guid sizeId)
    {
        var size = _sizes.FirstOrDefault(x => x.Id == sizeId);

        if (size == null)
            throw new DomainException("Size does not exist.");

        return size;
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
}



// TODO: Media — отдельный агрегат, а ProductVariant хранит только MainImageId.
//  Это уже уровень архитектурного разделения bounded contexts.
//  mark image as main

// TODO: создать коллекцию с IDs аттрибутов, и сделать проверку на макс колличество аттрибутов
//  if (_attributeIds.Count >= MAX_ATTRIBUTES)
//  ProductAttributeRules.MaxAttributesCountValidation(_attributes.Count);




// TODO:
//  throw new DomainException("Maximum number of attributes reached.");
//  if (Status != ProductVariantStatus.Published) - переносится в Application Layer
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