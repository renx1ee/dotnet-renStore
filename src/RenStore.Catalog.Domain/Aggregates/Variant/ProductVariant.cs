using RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Price;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;
using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;
using RenStore.Catalog.Domain.Aggregates.Variant.Rules;
using RenStore.Catalog.Domain.Aggregates.VariantDetails;
using RenStore.Catalog.Domain.Entities;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;
// TODO: 339 line

/// <summary>
/// Represents a product variant physical entity with lifecycle and invariants.
/// </summary>
public class ProductVariant
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    private readonly List<VariantSize> _sizes = new();
    private readonly List<Guid> _imageIds = new();
    private readonly List<Guid> _attributeIds = new();
    
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
    
    // вынести в отдельный контекст
    /*/// <summary>
    /// Customer rating and review score for this product variant.
    /// Calculated from user reviews.
    /// </summary>
    public Rating Rating { get; private set; } */
    
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
    
    /// <summary>
    /// The collection of images IDs associated with this variant.
    /// </summary>
    public IReadOnlyCollection<Guid> ImageIds => _imageIds.AsReadOnly();
    
    /// <summary>
    /// The collection of attributes IDs associated with this variant.
    /// </summary>
    public IReadOnlyCollection<Guid> AttributeIds => _attributeIds.AsReadOnly();
    
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
    
    public Guid AddSize(
        LetterSize letterSize,
        DateTimeOffset now)
    {
        EnsureNotDeleted();

        var activeSizes = _sizes
            .Where(s => !s.IsDeleted)
            .ToList();

        if (activeSizes.Any(x => x.Size.LetterSize == letterSize))
            throw new DomainException("The size already exits in the system.");
        
        //TODO:  нужно убедиться, что SizeType согласован с категорией продукта,
        // иначе можно добавить несовместимый размер.
        
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
    
    public void AddPriceToSize(
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
    }

    public void AddImageReference(
        DateTimeOffset now,
        Guid imageId)
    {
        EnsureNotDeleted();

        if (imageId == Guid.Empty)
            throw new DomainException("Image ID cannot be empty guid.");

        if (_imageIds.Contains(imageId))
            throw new DomainException("Image ID already contains.");

        Raise(new AddedImageReferenceEvent(
            OccurredAt: now,
            ImageId: imageId,
            VariantId: Id,
            EventId: Guid.NewGuid()));
    }
    
    public void RemoveImageReference(
        DateTimeOffset now,
        Guid imageId)
    {
        EnsureNotDeleted();

        if (imageId == Guid.Empty)
            throw new DomainException("Image ID cannot be empty guid.");

        if (!_imageIds.Contains(imageId))
            throw new DomainException("Image does not exists.");

        Raise(new RemoveImageReferenceEvent(
            OccurredAt: now,
            ImageId: imageId,
            VariantId: Id,
            EventId: Guid.NewGuid()));
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

    public void Publish(DateTimeOffset now)
    {
        EnsureNotDeleted();

        if (Status == ProductVariantStatus.Published)
            return;
            /*throw new DomainException("Variant has already been published.");*/
        
        if (!_imageIds.Any())
            throw new DomainException("Product variant must have at least one image.");

        if (MainImageId == Guid.Empty)
            throw new DomainException("Product variant must have main image.");

        var activeSizes = _sizes
            .Where(s => !s.IsDeleted)
            .ToList();

        if (!activeSizes.Any())
            throw new DomainException("Product variant must have at least one size.");

        foreach (var size in activeSizes)
        {
            var activePrice = size.Prices
                .FirstOrDefault(x => x.IsActive);
            
            if (activePrice is null)
                throw new DomainException($"Size: {size.Id} has no active price.");

            if (activePrice.Price.Amount <= 0)
                throw new DomainException($"Size: {size.Id} has invalid price.");
        }
        
        Raise(new VariantPublishedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            VariantId: Id));
    }
    
    public void Archive(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if (updatedById == Guid.Empty)
            throw new DomainException(
                "Updated By ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException(
                "Updated By role cannot be empty string.");
        
        if (Status == ProductVariantStatus.Archived)
            return;
        
        Raise(new VariantArchivedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            VariantId: Id));
    }
    
    public void ToDraft(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if (updatedById == Guid.Empty)
            throw new DomainException(
                "Updated By ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException(
                "Updated By role cannot be empty string.");
        
        if (Status == ProductVariantStatus.Draft)
            return;
        
        Raise(new VariantDraftedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            VariantId: Id));
    }
    
    public void Delete(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EnsureNotDeleted("Cannot delete already deleted variant.");
        
        if (updatedById == Guid.Empty)
            throw new DomainException(
                "Updated By ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException(
                "Updated By role cannot be empty string.");
        
        Raise(new VariantRemovedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            VariantId: Id,
            OccurredAt: now));
    }

    public void SetMainImageId(
        DateTimeOffset now,
        Guid imageId)
    {
        if (imageId == Guid.Empty)
            throw new DomainException("Main image ID cannot be guid empty.");
        
        if(!_imageIds.Contains(imageId))
            throw new DomainException("Image ID does not belong to this variant.");
        
        if(imageId == MainImageId) 
            return;
        
        Raise(new MainImageIdSetEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            VariantId: Id,
            ImageId: imageId));
    }
    
    public void RemoveSize(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now,
        Guid sizeId)
    {
        EnsureNotDeleted();
        
        if (updatedById == Guid.Empty)
            throw new DomainException(
                "Updated By ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException(
                "Updated By role cannot be empty string.");

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
    
    public void RestoreSize(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now,
        Guid sizeId)
    {
        EnsureNotDeleted();
        
        if (updatedById == Guid.Empty)
            throw new DomainException(
                "Updated By ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException(
                "Updated By role cannot be empty string.");
        
        var size = GetSize(sizeId);
        
        if(!size.IsDeleted)
            throw new DomainException("Size is not deleted.");
        
        Raise(new VariantSizeRestoredEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            VariantId: Id,
            SizeId: sizeId));
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
                        system: e.SizeSystem),// TODO:
                    variantId: e.VariantId));
                UpdatedAt = e.OccurredAt;
                break;
            
            case PriceCreatedEvent e:
                var createdSize = _sizes.SingleOrDefault(x => x.Id == e.SizeId)
                                  ?? throw new DomainException("Variant size is not found.");
                
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
                                  ?? throw new DomainException("Variant size is not found.");
                
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                removedSize.Delete(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
                break;
            
            case VariantSizeRestoredEvent e:
                var restoredSize = _sizes.SingleOrDefault(x => x.Id == e.SizeId)
                                  ?? throw new DomainException("Variant size is not found.");
                
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                restoredSize.Restore(e.OccurredAt);
                UpdatedAt = e.OccurredAt;
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
    
    /// <summary>
    /// Ensures the product variant is not deleted before performing operations.
    /// </summary>
    /// <param name="message">Optional custom error message.</param>
    /// <exception cref="DomainException">Thrown when entity is deleted.</exception>
    private void EnsureNotDeleted(string? message = null)
    {
        if (Status == ProductVariantStatus.Deleted)
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

// TODO: в application service вынести проверки инвариантов:
// Media — отдельный агрегат, а ProductVariant хранит только MainImageId.
//  Это уже уровень архитектурного разделения bounded contexts.
//  mark image as main
// создать коллекцию с IDs аттрибутов, и сделать проверку на макс колличество аттрибутов
//  if (_attributeIds.Count >= MAX_ATTRIBUTES)
//  ProductAttributeRules.MaxAttributesCountValidation(_attributes.Count);
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