using RenStore.Catalog.Domain.Aggregates.Variant.Events;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;
using RenStore.Catalog.Domain.Aggregates.Variant.Rules;
using RenStore.Catalog.Domain.Aggregates.VariantDetails;
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
    /// Image Unique Identifier //TODO:
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

    public void SetMainImageId(
        DateTimeOffset now,
        Guid imageId)
    {
        if (imageId == Guid.Empty)
            throw new DomainException("Main image ID cannot be guid empty.");
        
        if(imageId == MainImageId) 
            return;
        
        Raise(new MainImageIdSet(
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));
    }

    #region size

    /// <summary>
    /// Adds a new size option with inventory to this product variant.
    /// Each variant can have multiple size options with independent stock levels.
    /// </summary>
    /// <param name="letterSize">Alphanumeric size designation (e.g., "M", "10", "42")</param>
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
        DateTimeOffset now)
    {
        EnsureNotDeleted();

        if (_sizes.Any(x => x.Size.LetterSize == letterSize))
            throw new DomainException("The size already exits in the system.");
        
        //TODO:  нужно убедиться, что SizeType согласован с категорией продукта,
        // иначе можно добавить несовместимый размер.
        
        Size.Validate(
            size: letterSize, 
            type: SizeType, 
            system: SizeSystem); 
        
        var sizeId = Guid.NewGuid();
        
        Raise(new VariantSizeCreated(
            OccurredAt: now,
            SizeId: sizeId,
            VariantId: Id,
            LetterSize: letterSize,
            SizeSystem: SizeSystem,
            SizeType: SizeType));
    }
    
    /// <summary>
    /// Removes a specific size option from this product variant's available selections.
    /// Used when a size is discontinued, out of stock permanently, or no longer manufactured.
    /// </summary>
    /// <param name="now">Timestamp for deletion audit</param>
    /// <param name="sizeId">Identifier of the size option to remove</param>
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
        Guid sizeId)
    {
        EnsureNotDeleted();

        var size = GetSize(sizeId);
        SizeEnsureNotDeleted(size);
        
        Raise(new VariantSizeRemoved(
            OccurredAt: now,
            VariantId: Id,
            VariantSizeId: sizeId));
    }
    
    /// <summary>
    /// Restores a previously deleted size option, making it available for purchase again.
    /// Reverses the effect of <see cref="DeleteSize"/> while preserving the size's configuration.
    /// </summary>
    /// <param name="now">Timestamp for restoration audit</param>
    /// <param name="sizeId">Identifier of the size option to restore</param>
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
        Guid sizeId)
    {
        EnsureNotDeleted();
        
        var size = GetSize(sizeId);
        SizeEnsureNotDeleted(size);
        
        Raise(new VariantSizeRestored(
            OccurredAt: now,
            VariantId: Id,
            VariantSizeId: sizeId));
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
                Url = e.Url;
                SizeSystem = e.SizeSystem;
                SizeType = e.SizeType;
                Status = ProductVariantStatus.Draft;
                Rating = Rating.Empty();
                break;
            
            case VariantSizeCreated e:
                _sizes.Add(VariantSize.Create(
                    id: e.SizeId,
                    now: e.OccurredAt,
                    size: Size.Create(
                        size: e.LetterSize, 
                        type: e.SizeType, 
                        system: e.SizeSystem),// TODO:
                    variantId: e.VariantId));
                UpdatedAt = e.OccurredAt;
                break;
            
            case MainImageIdSet e:
                MainImageId = e.ImageId;
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

// TODO:
// if (variantSizeRepository.ExistsByVariantId(variantId))
// throw new DomainException("Size already exists");
// Уникальный индекс в БД (практичный)
// UNIQUE (VariantId)
// UNIQUE (VariantId, LetterSize)