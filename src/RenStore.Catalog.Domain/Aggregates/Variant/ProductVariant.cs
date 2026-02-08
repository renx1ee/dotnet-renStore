using RenStore.Catalog.Domain.Aggregates.Variant.Events;
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
    private readonly List<ProductAttribute> _attributes = new();
    private readonly List<ProductPriceHistory> _priceHistory = new();
    private readonly List<ProductImage> _images = new();
    private readonly List<VariantSize> _sizes = new();
    
    private Color _color;
    private ProductDetail _productDetails;
    
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
    /// Available inventory quantity for this product variant.
    /// </summary>
    public int InStock { get; private set; }
    
    /// <summary>
    /// Total number of unit sold for this product variant. 
    /// </summary>
    /// <remarks>
    /// Include on each successful order.
    /// Does not include cancelled or returned items. // TODO:
    /// </remarks>
    public int Sales { get; private set; }
    
    /// <summary>
    /// Current lifecycle state on this product variant.
    /// </summary>
    public ProductVariantStatus Status { get; private set; }
    
    /// <summary>
    /// Indicates whether this variant is currently available for purchase.
    /// </summary>
    public bool IsAvailable { get; private set; }  
    
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
    public IReadOnlyCollection<ProductAttribute> ProductAttributes => _attributes.AsReadOnly();
    
    /// <summary>
    /// The collection of price history associated with this variant.
    /// </summary>
    public IReadOnlyCollection<ProductPriceHistory> PriceHistories => _priceHistory.AsReadOnly();
    
    /// <summary>
    /// The collection of images associated with this variant.
    /// </summary>
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();
    
    /// <summary>
    /// The collection of sizes associated with this variant.
    /// </summary>
    public IReadOnlyCollection<VariantSize> Sizes => _sizes.AsReadOnly();
    
    private ProductVariant() { }
    
    /// <summary>
    /// Initializes or updates a product variant aggregate with the specified values.
    /// Ensures that the aggregate invariants are respected and records necessary events.
    /// </summary>
    /// <param name="id">Unique identifier of the variant product aggregate.</param>
    /// <param name="productId">Unique identifier of the product.</param>
    /// <param name="colorId">Unique identifier of the color.</param>
    /// <param name="name">Display the name of this specific variant.</param>
    /// <param name="normalizedName">Uppercase normalized version of the variant name for case-insensitive operations.</param>
    /// <param name="rating">Customer rating and review score for this product variant.</param>
    /// <param name="article">Internal article number that uniquely identifies this product variant.</param>
    /// <param name="inStock">Available inventory quantity for this product variant.</param>
    /// <param name="sales">Total number of unit sold for this product variant.</param>
    /// <param name="isAvailable">Indicates whether this variant is currently available for purchase.</param>
    /// <param name="url">SEO-friendly URL slug for this product variant.</param>
    /// <param name="sizeSystem">Measurement system used for sizing in this product variant.</param>
    /// <param name="sizeType">Category of sizing applicable to this product variant.</param>
    /// <param name="createdAt">Date when the product was created.</param>
    /// <param name="uploadAt">Date when the product was updated.</param>
    /// <param name="deletedAt">Date when the product was deleted.</param>
    /// <returns></returns>
    public static ProductVariant Reconstitute(
        Guid id,
        Guid productId,
        int colorId,
        string name,
        string normalizedName,
        Rating rating,
        long article,
        int inStock,
        int sales,
        bool isAvailable,
        string url,
        SizeSystem sizeSystem,
        SizeType sizeType,
        DateTimeOffset createdAt,
        DateTimeOffset? uploadAt,
        DateTimeOffset? deletedAt)
    {
        var variant = new ProductVariant()
        {
            Id = id,
            ProductId = productId,
            ColorId = colorId,
            Name = name,
            NormalizedName = normalizedName,
            Rating = rating,
            Article = article,
            Sales = sales,
            IsAvailable = isAvailable,
            SizeSystem = sizeSystem,
            SizeType = sizeType,
            InStock = inStock,
            Url = url,
            CreatedAt = createdAt,
            UpdatedAt = uploadAt,
            DeletedAt = deletedAt
        };

        return variant;
    }
    
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
        int inStock,
        SizeSystem sizeSystem,
        SizeType sizeType,
        string url)
    {
        ProductVariantRules.ValidateProductId(productId);
        ProductVariantRules.ValidateColorId(colorId);

        var trimmedName = name.Trim();
        ProductVariantRules.ValidateName(trimmedName);
        
        ProductVariantRules.ValidateInStock(inStock);

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
                InStock: inStock,
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
        
        if (_productDetails != null)
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
            ModelFeatures: trimmedModelFeatures ?? null,
            DecorativeElements: trimmedDecorativeElements ?? null,
            Equipment: trimmedEquipment ?? null,
            Description: trimmedDescription,
            Composition: trimmedComposition,
            CaringOfThings: trimmedCaringOfThings ?? null,
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
        VariantSizeRules.ProductVariantIdValidate(Id);
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
    
    /// <summary>
    /// Adds a product image to this variant with metadata and display rules.
    /// Images are used for visual presentation in catalog listings and product pages.
    /// </summary>
    /// <param name="now">Timestamp for creation audit</param>
    /// <param name="originalFileName">Original uploaded file name</param>
    /// <param name="storagePath">Path to stored image file</param>
    /// <param name="fileSizeBytes">Size of image file in bytes</param>
    /// <param name="isMain">Whether this is the primary display image</param>
    /// <param name="sortOrder">Display order in image gallery (lower = first)</param>
    /// <param name="weight">Image width in pixels</param>
    /// <param name="height">Image height in pixels</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Variant is deleted
    /// - Maximum image limit reached
    /// - Image metadata fails validation (size, dimensions, etc.)
    /// </exception>
    /// <remarks>
    /// Only one image can be marked as main. If <paramref name="isMain"/> is true,
    /// any existing main image will be demoted. Images are displayed according to sort order.
    /// </remarks>
    public void AddImage(
        DateTimeOffset now,
        string originalFileName,
        string storagePath,
        long fileSizeBytes,
        bool isMain,
        short sortOrder,
        int weight, 
        int height)
    {
        EnsureNotDeleted();

        ProductVariantRules.MaxImagesCountValidation(_images.Count);

        var imageId = Guid.NewGuid();
        
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
    
    // TODO:
    public void ChangeColor()
    {
        EnsureNotDeleted();
    }
    
    /// <summary>
    /// Publishes this variant, making it visible and available for purchase in the catalog.
    /// Enforces business rules requiring complete product information before publication.
    /// </summary>
    /// <param name="now">Timestamp for publication</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Variant is deleted
    /// - Variant has no images (visual representation required)
    /// - Variant has no product details (specifications required)
    /// </exception>
    /// <remarks>
    /// Publishing is a business transaction that makes the variant available to customers.
    /// Unpublished variants remain in the system but are not visible in the public catalog.
    /// </remarks>
    public void Publish(DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if (!_images.Any())
            throw new DomainException("Variant must have images.");
        
        if (_productDetails == null)
            throw new DomainException("Variant must have details.");
        
        Raise(new VariantPublished(
            VariantId: Id,
            OccurredAt: now));
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
    /// Increases the available inventory quantity for this variant.
    /// Used for restocking from suppliers or correcting inventory discrepancies.
    /// </summary>
    /// <param name="now">Timestamp for stock update</param>
    /// <param name="count">Number of units to add to inventory (must be positive)</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Variant is deleted
    /// - Count is zero or negative
    /// </exception>
    /// <remarks>
    /// Adding stock may automatically re-enable availability if the variant was out of stock.
    /// Stock additions are additive and can be performed multiple times.
    /// </remarks>
    public void AddToStock(
        DateTimeOffset now,
        int count)
    {
        EnsureNotDeleted();

        if (count <= 0)
            throw new DomainException("Cannot sell 0 or less products.");

        Raise(new VariantStockAdded(
            OccurredAt: now,
            VariantId: Id,
            Count: count));
    }
    
    /// <summary>
    /// Decreases the available inventory quantity for this variant.
    /// Used for order fulfillment, inventory adjustments, or returns processing.
    /// </summary>
    /// <param name="now">Timestamp for stock update</param>
    /// <param name="count">Number of units to remove from inventory</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Variant is deleted
    /// - Variant is available and count is zero or negative
    /// - Requested count exceeds available inventory
    /// </exception>
    /// <remarks>
    /// When inventory reaches zero, the variant automatically becomes unavailable for purchase.
    /// This method validates stock availability before processing the removal.
    /// </remarks>
    public void RemoveFromStock(
        DateTimeOffset now,
        int count)
    {
        EnsureNotDeleted();

        if (IsAvailable && count <= 0)
            throw new DomainException("Cannot sell 0 or less products.");
        
        if(count > InStock)
            throw new DomainException("The count of sells exceed available count.");

        Raise(new VariantRemovedFromStock(
            OccurredAt: now,
            VariantId: Id,
            Count: count));
    }

    /// <summary>
    /// Records a customer purchase of this variant, updating sales metrics.
    /// This is a business transaction representing actual sales to customers.
    /// </summary>
    /// <param name="now">Timestamp of the sale</param>
    /// <param name="count">Number of units purchased in this transaction</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Variant is deleted
    /// - Count is zero or negative
    /// - Insufficient stock available (handled by stock validation in order processing)
    /// </exception>
    /// <remarks>
    /// Sales are recorded separately from stock adjustments to track revenue and popularity.
    /// Stock reduction should be handled through <see cref="RemoveFromStock"/> in a coordinated transaction.
    /// </remarks>
    public void Sell(
        DateTimeOffset now,
        int count)
    {
        EnsureNotDeleted();
        
        Raise(new SaleOfVariantOccurred(
            OccurredAt: now,
            VariantId: Id,
            Count: count));
    }
    // TODO:
    public void CancelSell()
    {
        
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
        
        Raise(new VariantAverageRatingUpdated(
            VariantId: Id,
            OccurredAt: now,
            Score: score));
    }
    
    /// <summary>
    /// Explicitly sets the availability status of this variant for purchase.
    /// Overrides automatic availability determined by stock levels and business rules.
    /// </summary>
    /// <param name="isAvailable">New availability status</param>
    /// <param name="now">Timestamp for status change</param>
    /// <exception cref="DomainException">
    /// Thrown when variant is deleted
    /// </exception>
    /// <remarks>
    /// This manual control allows:
    /// - Temporarily disabling a variant (e.g., for maintenance)
    /// - Seasonal availability management
    /// - Manual re-enabling despite zero stock
    /// 
    /// No change occurs if the requested status matches current status.
    /// </remarks>
    public void SetAvailability(
        bool isAvailable,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if(IsAvailable == isAvailable) return;
        
        Raise(new VariantAvailabilityUpdated(
            OccurredAt: now,
            VariantId: Id,
            IsAvailable: isAvailable));
    }
    
    /// <summary>
    /// Designates a specific image as the primary display image for this variant.
    /// The main image is featured prominently in catalog listings and product pages.
    /// </summary>
    /// <param name="now">Timestamp for status change</param>
    /// <param name="imageId">Identifier of the image to promote</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Variant is deleted
    /// - Image with specified ID is not found
    /// - Image is already deleted
    /// </exception>
    /// <remarks>
    /// If another image is currently marked as main, it will be demoted automatically.
    /// No action is taken if the specified image is already the main image.
    /// Only one image can be designated as main at any time.
    /// </remarks>
    public void MarkImageAsMain(
        DateTimeOffset now,
        Guid imageId)
    {
        EnsureNotDeleted();

        var currentImage = _images.FirstOrDefault(x => x.Id == imageId);

        if (currentImage == null)
            throw new DomainException("Image was not found.");
        
        if(currentImage.IsDeleted)
            throw new DomainException("Cannot mark already deleted image as main.");
        
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
        var existingAttribute = _attributes.FirstOrDefault(x => x.Id == attributeId);

        if (existingAttribute == null)
            throw new DomainException("Cannot delete not exists variant.");
        
        
        Raise(new VariantAttributeRemoved(
            OccurredAt: now,
            VariantId: Id,
            AttributeId: attributeId));
    }
    
    /// <summary>
    /// Marks a product image as deleted, removing it from display in the catalog.
    /// Implements soft deletion for potential recovery and audit trail.
    /// </summary>
    /// <param name="now">Timestamp for deletion audit</param>
    /// <param name="imageId">Identifier of the image to delete</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Image with specified ID is not found
    /// - Image is already marked as deleted
    /// </exception>
    /// <remarks>
    /// If the deleted image was marked as main, the main image status should be
    /// reassigned to another available image or cleared.
    /// Deleted images may be retained in storage for compliance or recovery purposes.
    /// </remarks>
    public void DeleteImage(
        DateTimeOffset now,
        Guid imageId)
    {
        var existingImage = _images.FirstOrDefault(x => x.Id == imageId);

        if (existingImage == null)
            throw new DomainException("Product image not exists.");
        
        if (existingImage.IsDeleted)
            throw new DomainException("The image already was deleted.");
        
        Raise(new VariantImageRemoved(
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));
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
        var existingSize = _sizes.FirstOrDefault(x => x.Id == variantSizeId);

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
        var existingAttribute = _attributes.FirstOrDefault(x => x.Id == attributeId);

        if (existingAttribute == null)
            throw new DomainException("Cannot delete not exists variant.");
        
        Raise(new VariantAttributeRestored(
            OccurredAt: now,
            VariantId: Id,
            AttributeId: attributeId));
    }
    
    /// <summary>
    /// Restores a previously deleted product image, making it visible in the catalog again.
    /// Reverses the effect of <see cref="DeleteImage"/> while preserving the image's metadata.
    /// </summary>
    /// <param name="now">Timestamp for restoration audit</param>
    /// <param name="imageId">Identifier of the image to restore</param>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Image with specified ID is not found
    /// - Image is not currently marked as deleted
    /// </exception>
    /// <remarks>
    /// Restored images regain their previous display position and properties.
    /// The image file must still exist in storage for successful restoration.
    /// Restoration does not automatically re-mark the image as main if it was previously.
    /// </remarks>
    public void RestoreImage(
        DateTimeOffset now,
        Guid imageId)
    {
        var existingImage = _images.FirstOrDefault(x => x.Id == imageId);

        if (existingImage == null)
            throw new DomainException("Product image not exists.");
        
        if (!existingImage.IsDeleted)
            throw new DomainException("The image was not deleted.");
        
        Raise(new VariantImageRestored(
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));
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
                InStock = e.InStock;
                Url = e.Url;
                IsAvailable = false;
                SizeSystem = e.SizeSystem;
                SizeType = e.SizeType;
                break;
            
            case VariantDetailsCreated e:
                _productDetails = ProductDetail.Create(
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
                _attributes.Add(ProductAttribute.Create(
                    key: e.Key,
                    value: e.Value,
                    productVariantId: e.VariantId,
                    now: e.OccurredAt));
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageCreated e:
                _images.Add(ProductImage.Create(
                    now: e.OccurredAt,
                    imageId: e.ImageId,
                    productVariantId: e.VariantId,
                    originalFileName: e.OriginalFileName,
                    storagePath: e.StoragePath,
                    fileSizeBytes: e.FileSizeBytes,
                    isMain: e.IsMain,
                    sortOrder: e.SortOrder,
                    weight: e.Weight,
                    height: e.Height));
                
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
            
            case VariantNameUpdated e:
                Name = e.Name;
                NormalizedName = e.Name.ToUpperInvariant();
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantStockAdded e:
                InStock += e.Count;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantRemovedFromStock e:
                InStock -= e.Count;

                if (InStock <= 0)
                    IsAvailable = false;
                
                UpdatedAt = e.OccurredAt;
                break;
            
            case SaleOfVariantOccurred e:
                RemoveFromStock(e.OccurredAt, e.Count);
                Sales += e.Count;
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantAverageRatingUpdated e:
                Rating = Rating.Add(e.Score);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantAvailabilityUpdated e:
                IsAvailable = e.IsAvailable;
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
            
            case VariantAttributeRemoved e:
                _attributes
                    .Single(x => x.Id == e.AttributeId)
                    .Delete(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageRemoved e:
                _images
                    .Single(x => x.Id == e.ImageId)
                    .Delete(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantSizeRemoved e:
                _sizes
                    .Single(x => x.Id == e.VariantSizeId)
                    .Delete(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantImageRestored e:
                _images
                    .Single(x => x.Id == e.ImageId)
                    .Restore(e.OccurredAt);
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
        }
    }
    
    /// <summary>
    /// Ensures the attribute is not deleted before performing operations.
    /// </summary>
    /// <param name="message">Optional custom error message</param>
    /// <exception cref="DomainException">Thrown when attribute is deleted</exception>
    private void EnsureNotDeleted(string? message = null)
    {
        if (Status == ProductVariantStatus.IsDeleted)
            throw new DomainException(message ?? "Entity is deleted.");
    }
}

// TODO: сделать создание
//  1) цвета, истории цен,
//  2) сделат методы для изменеия SizeType, SizeSystem (тобиж изменение таблицы размеров)