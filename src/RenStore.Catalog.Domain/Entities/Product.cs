using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.Events.Product;
using RenStore.SharedKernal.Domain.Exceptions;
using RenStore.SharedKernal.Domain.ValueObjects;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a product physical entity with lifecycle and invariants.
/// </summary>
public class Product
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    private List<Guid> _productVariantIds = new();

    public Guid Id { get; private set; }
    public Rating? OverallRating { get; private set; } // TODO: доделать
    public long SellerId { get; private set; }
    public int SubCategoryId { get; private set; }
    public ProductStatus Status { get; private set; } // TODO: доделать статус
    public DateTimeOffset CreatedAt { get; private set; }
    public IReadOnlyCollection<Guid> ProductVariantIds => _productVariantIds.AsReadOnly();
    
    private const int MaxVariantsCount = 50;
    
    private Product() { }
    
    public static Product Reconstitute(
        Guid id,
        Rating overallRating,
        long sellerId,
        int subCategoryId,
        ProductStatus status,
        bool isDeleted,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt,
        DateTimeOffset? deletedAt)
    {
        var product = new Product()
        {
            Id = id,
            OverallRating = overallRating,
            SellerId = sellerId,
            SubCategoryId = subCategoryId,
            Status = status,
            CreatedAt = createdAt,
            
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt
        };

        return product;
    }
    
    // Create и присваивает поля
    // и Raise делает то же самое через Apply
    public static Product Create(
        long sellerId,
        int subCategoryId,
        DateTimeOffset now)
    {
        SellerIdValidate(sellerId);

        SubCategoryIdValidate(subCategoryId);
        
        var product = new Product()
        {
            Id = Guid.NewGuid(),
            SellerId = sellerId,
            SubCategoryId = subCategoryId,
            Status = ProductStatus.PendingModeration,
            IsDeleted = false,
            CreatedAt = now
        };
        
        product.Raise(new ProductCreated(
                ProductId: product.Id,
                Status: product.Status,
                SellerId: sellerId,
                SubCategoryId: subCategoryId,
                OccurredAt: now));

        return product;
    }
    
    public void AddVariantReference(
        Guid variantId,
        DateTimeOffset now)
    {
        EnsureNotDeleted();

        VariantValidate(variantId);
        
        Raise(new ProductVariantReferenceCreated(
            ProductId: Id,
            ProductVariantId: variantId,
            OccurredAt: now));
    }
    
    // todo: защитить от повторного вызова
    public void Publish(DateTimeOffset now)
    {
        if (!_productVariantIds.Any())
            throw new DomainException("Product must have variants.");

        Raise(new ProductPublished(
            ProductId: Id,
            OccurredAt: now));
    }
    // TODO: сделать общий рейтинг
    public void Rate()
    {
        
    }
    
    public void Delete(DateTimeOffset now)
    {
        if(IsDeleted)
            throw new DomainException("Cannot delete deleted entity.");
        
        Raise(new ProductRemoved(
            ProductId: Id, 
            OccurredAt: now));
    }
    
    public void DeleteVariantReference(
        Guid variantId,
        DateTimeOffset now)
    {
        ProductVariantReferenceExists(variantId);
       
        Raise(new ProductVariantReferenceRemoved(
            ProductId: Id,
            ProductVariantId: variantId,
            OccurredAt: now));
    }
    
    public void Restore(DateTimeOffset now)
    {
        if(!IsDeleted)
            throw new DomainException("Cannot restore active entity.");
        
        Raise(new ProductRestored(
            ProductId: Id, 
            OccurredAt: now));
    }
    
    protected override void Apply(object @event)
    {
        switch (@event)
        {
            case ProductCreated e:
                Id = e.ProductId;
                SellerId = e.SellerId;
                SubCategoryId = e.SubCategoryId;
                Status = e.Status;
                IsDeleted = false;
                CreatedAt = e.OccurredAt;
                break;
            
            case ProductPublished e:
                Status = ProductStatus.Published;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductRemoved e:
                IsDeleted = true;
                UpdatedAt = e.OccurredAt;
                DeletedAt = e.OccurredAt;
                break;
            
            case ProductRestored e:
                IsDeleted = false;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductVariantReferenceCreated e:
                UpdatedAt = e.OccurredAt;
                _productVariantIds.Add(e.ProductVariantId);
                break;
            
            case ProductVariantReferenceRemoved e:
                UpdatedAt = e.OccurredAt;
                _productVariantIds.Remove(e.ProductVariantId);
                break;
        }
    }
    
    private void ProductVariantReferenceExists(Guid variantId)
    {
        if(!_productVariantIds.Contains(variantId))
            throw new DomainException("Variant does not exist.");
    }
    
    private void VariantValidate(Guid variantId)
    {
        if (_productVariantIds.Count >= MaxVariantsCount)
            throw new DomainException($"Product variants count must be less then {MaxVariantsCount}.");
        
        if(_productVariantIds.Contains(variantId))
            throw new DomainException("Product variants reference already exists.");
    }
    
    private static void SellerIdValidate(long sellerId)
    {
        if (sellerId <= 0)
            throw new DomainException("Seller ID must be more than 0.");
    }
    
    private static void SubCategoryIdValidate(int subCategoryId)
    {
        if (subCategoryId <= 0)
            throw new DomainException("Sub Category ID must be more than 0.");
    }
}