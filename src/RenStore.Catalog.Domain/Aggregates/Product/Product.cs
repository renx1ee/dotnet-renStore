using RenStore.Catalog.Domain.Aggregates.Product.Events;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Exceptions;
using RenStore.SharedKernal.Domain.ValueObjects;

namespace RenStore.Catalog.Domain.Aggregates.Product;

/// <summary>
/// Represents a product physical entity with lifecycle and invariants.
/// </summary>
public class Product
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    private List<Guid> _productVariantIds = new();
    
    /// <summary>
    /// Unique identifier of the product.
    /// </summary>
    public Guid Id { get; private set; }
    
    /*/// <summary>
    /// Overall rating calculated of all product variants.
    /// </summary>
    public Rating OverallRating { get; private set; } */
    
    /// <summary>
    /// Current lifecycle status of the product.
    /// </summary>
    public ProductStatus Status { get; private set; }
    
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
    /// Unique identifier of the seller.
    /// </summary>
    public long SellerId { get; private set; }
    
    /// <summary>
    /// Unique identifier of the sub category.
    /// </summary>
    public Guid SubCategoryId { get; private set; }
    
    /// <summary>
    /// The collection of product variant identifiers associated with this product.
    /// </summary>
    public IReadOnlyCollection<Guid> ProductVariantIds => _productVariantIds.AsReadOnly(); 
    
    private const int MaxVariantsCount = 50;
    
    private Product() { }
    
    /// <summary>
    /// Create a new product in the system, linked to a specific seller.
    /// The method checks business-rules:
    /// - The seller must exist and have the right to sell goods;
    /// - The sub category must be valid;
    /// - The creation date is fixed to the current time to ensure correct history;
    /// </summary>
    /// <param name="sellerId">The unique seller identifier - owner of the product. The seller must be registered and active.</param>
    /// <param name="subCategoryId">The unique sub category identifier to which the product belong. The sub category must be existed in the system.</param>
    /// <param name="now">Timestamp when the operation occurs. Used for event history.</param>
    /// <returns>Created product entity with established business invariants.</returns>
    public static Product Create(
        long sellerId,
        Guid subCategoryId,
        DateTimeOffset now)
    {
        SellerIdValidate(sellerId);
        SubCategoryIdValidate(subCategoryId);

        var product = new Product();
        var productId = Guid.NewGuid();
        
        product.Raise(new ProductCreatedEvent(
            EventId: Guid.NewGuid(), 
            ProductId: productId,
            Status: ProductStatus.PendingModeration,
            SellerId: sellerId,
            SubCategoryId: subCategoryId,
            OccurredAt: now));

        return product;
    }
    
    /// <summary>
    /// Adds a variant to the product aggregate.
    /// Ensure domain rules are enforced:
    /// - The variant must exist in the system.
    /// - A variant cannot be added more than once to the same product.
    /// </summary>
    /// <param name="variantId">Unique product variant id. The product variant ID must be existed in the system.</param>
    /// <param name="now">Timestamp when the operation occurs. Used for the event history of the unit.</param>
    public void AddVariantReference(
        Guid variantId,
        DateTimeOffset now)
    {
        this.EndureNotDeleted();

        VariantIdsValidate(variantId);
        
        Raise(new ProductVariantReferenceCreatedEvent(
            EventId: Guid.NewGuid(), 
            ProductId: Id,
            VariantId: variantId,
            OccurredAt: now));
    }
    
    /*/// <summary>
    /// Register a new rating for the product.
    /// </summary>
    /// <param name="now">Timestamp when the operation occurs. Used for the event history of the unit.</param>
    /// <param name="score">Rating value assigned to the product.</param>
    public void Rate(
        DateTimeOffset now,
        decimal score)
    {
        EndureNotDeleted();
        
        Raise(new ProductAverageRatingUpdated(
            ProductId: Id,
            OccurredAt: now,
            Score: score));
    }*/
    
    /// <summary>
    /// Publish the product in the system.
    /// </summary>
    /// <param name="now">Timestamp when the operation occurs. Used for the event history of the unit.</param>
    /// <exception cref="DomainException"></exception>
    public void MarkAsPublished(DateTimeOffset now)
    {
        EndureNotDeleted();

        if (!_productVariantIds.Any())
        {
            throw new DomainException(
                "Product must have variants.");
        }

        if (Status == ProductStatus.Published)
        {
            throw new DomainException(
                "Product is already published.");
        }

        Raise(new ProductPublishedEvent(
            EventId: Guid.NewGuid(), 
            Status: ProductStatus.Published,
            ProductId: Id,
            OccurredAt: now));
    }
    
    /// <summary>
    /// Mark the product as rejected.
    /// Updates the product aggregate state and records the changes in the domain event history.
    /// </summary>
    /// <param name="now">Timestamp when the operation occurs. Used for the event history of the unit.</param>
    public void MarkAsRejected(DateTimeOffset now)
    {
        EndureNotDeleted();
        
        Raise(new ProductRejectedEvent(
            EventId: Guid.NewGuid(), 
            Status: ProductStatus.Rejected,
            ProductId: Id,
            OccurredAt: now));
    }
    
    /// <summary>
    /// Mark the product as approved.
    /// Update the product aggregate state and record the changes in the domain event history.
    /// </summary>
    /// <param name="now">Timestamp when the operation occurs. Used for the event history of the unit.</param>
    public void MarkAsApproved(DateTimeOffset now)
    {
        EndureNotDeleted();
        
        Raise(new ProductApprovedEvent(
            EventId: Guid.NewGuid(), 
            Status: ProductStatus.Approved,
            ProductId: Id,
            OccurredAt: now));
    }
    
    /// <summary>
    /// Mark the product as draft.
    /// Update the product aggregate state and record the changes in the domain event history.
    /// </summary>
    /// <param name="now">Timestamp when the operation occurs. Used for the event history of the unit.</param>
    public void MarkAsDraft(DateTimeOffset now)
    {
        EndureNotDeleted();
        
        Raise(new ProductMovedToDraftEvent(
            EventId: Guid.NewGuid(), 
            Status: ProductStatus.Draft,
            ProductId: Id,
            OccurredAt: now));
    }
    
    /// <summary>
    /// Mark the product as archived.
    /// Update the product aggregate state and record the changes in the domain event history.
    /// </summary>
    /// <param name="now">Timestamp when the operation occurs. Used for the event history of the unit.</param>
    public void MarkAsArchived(DateTimeOffset now)
    {
        EndureNotDeleted();
        
        Raise(new ProductArchivedEvent(
            EventId: Guid.NewGuid(), 
            Status: ProductStatus.Archived,
            ProductId: Id,
            OccurredAt: now));
    }
    
    /// <summary>
    /// Mark the product as hidden.
    /// Update the product aggregate state and record the changes in the domain event history.
    /// </summary>
    /// <param name="now">Timestamp when the operation occurs. Used for the event history of the unit.</param>
    public void MarkAsHidden(DateTimeOffset now)
    {
        EndureNotDeleted();
        
        Raise(new ProductHiddenEvent(
            EventId: Guid.NewGuid(), 
            Status: ProductStatus.Hidden,
            ProductId: Id,
            OccurredAt: now));
    }
    
    /// <summary>
    /// Soft-deletes the product.
    /// Mark the product as inactive while preserving its data and records the operation in the domain event history.
    /// </summary>
    /// <param name="now">Timestamp when the operation occurs. Used for the event history of the unit.</param>
    public void Delete(DateTimeOffset now)
    {
        EndureNotDeleted();
        
        Raise(new ProductRemovedEvent(
            EventId: Guid.NewGuid(), 
            Status: ProductStatus.Deleted,
            ProductId: Id, 
            OccurredAt: now));
    }
    
    /// <summary>
    /// Removes the association between the product and a variant.
    /// Updates the aggregate state and records the change in the domain event history.
    /// </summary>
    /// <param name="variantId">Unique product variant id. The product variant ID must be existed in the system.</param>
    /// <param name="now">Timestamp when the operation occurs. Used for the event history of the unit.</param>
    public void RemoveVariantReference(
        Guid variantId,
        DateTimeOffset now)
    {
        EndureNotDeleted();
        
        ProductVariantReferenceExists(variantId);
       
        Raise(new ProductVariantReferenceRemovedEvent(
            EventId: Guid.NewGuid(), 
            ProductId: Id,
            VariantId: variantId,
            OccurredAt: now));
    }
    
    /// <summary>
    /// Removes the association between the product and a variant.
    /// Updates the aggregate state and records the change in the domain event history.
    /// </summary>
    /// <param name="now">Timestamp when the operation occurs. Used for the event history of the unit.</param>
    /// <exception cref="DomainException"></exception>
    public void Restore(DateTimeOffset now)
    {
        if(Status != ProductStatus.Deleted)
            throw new DomainException(
                "Cannot restore active entity.");
        
        Raise(new ProductRestoredEvent(
            EventId: Guid.NewGuid(),
            Status: ProductStatus.Draft,
            ProductId: Id,
            OccurredAt: now));
    }
    
    protected override void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case ProductCreatedEvent e:
                Id = e.ProductId;
                SellerId = e.SellerId;
                SubCategoryId = e.SubCategoryId;
                Status = e.Status;
                CreatedAt = e.OccurredAt;
                break;
            
            case ProductPublishedEvent e:
                Status = e.Status;
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductRejectedEvent e:
                Status = e.Status; 
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductApprovedEvent e:
                Status = e.Status; 
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductMovedToDraftEvent e:
                Status = e.Status; 
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductArchivedEvent e:
                Status = e.Status; 
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductHiddenEvent e:
                Status = e.Status; 
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductRemovedEvent e:
                Status = e.Status; 
                UpdatedAt = e.OccurredAt;
                DeletedAt = e.OccurredAt;
                break;
            
            case ProductRestoredEvent e:
                Status = e.Status; 
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductVariantReferenceCreatedEvent e:
                _productVariantIds.Add(e.VariantId);
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductVariantReferenceRemovedEvent e:
                _productVariantIds.Remove(e.VariantId);
                UpdatedAt = e.OccurredAt;
                break;
        }
    }

    public static Product Rehydrate(IEnumerable<IDomainEvent> history)
    {
        var product = new Product();

        foreach (var @event in history)
        {
            product.Apply(@event);
            product.Version++;
        }

        return product;
    }

    private void EndureNotDeleted()
    {
        if(Status == ProductStatus.Deleted)
            throw new DomainException("Product already was deleted.");
    }
    
    private void ProductVariantReferenceExists(Guid variantId)
    {
        if(!_productVariantIds.Contains(variantId))
            throw new DomainException("Variant does not exist.");
    }
    
    private void VariantIdsValidate(Guid variantId)
    {
        if (_productVariantIds.Count >= MaxVariantsCount)
            throw new DomainException($"Product variants count must be less than {MaxVariantsCount}.");

        if (variantId == Guid.Empty)
            throw new DomainException("Product variant Id cannot be empty guid.");
        
        if(_productVariantIds.Contains(variantId))
            throw new DomainException("Product variants reference already exists.");
    }
    
    private static void SellerIdValidate(long sellerId)
    {
        if (sellerId <= 0)
            throw new DomainException("Seller ID must be more than 0.");
    }
    
    private static void SubCategoryIdValidate(Guid subCategoryId)
    {
        if (subCategoryId == Guid.Empty)
            throw new DomainException("Sub Category ID cannot be empty guid.");
    }
}

/*/// <summary>
    /// Initializes or updates a product aggregate with the specified values.
    /// Ensures that the aggregate invariants are respected and records necessary events.
    /// </summary>
    /// <param name="id">Unique identifier of the product aggregate.</param>
    /// <param name="overallRating">Overall rating derived from all product variants.</param>
    /// <param name="sellerId">Identifier of the seller who owns the product.</param>
    /// <param name="subCategoryId">Identifier of the subcategory the product belongs to.</param>
    /// <param name="status">Current lifecycle status of the product (e.g., Pending, Approved, Rejected).</param>
    /// <param name="createdAt">Timestamp when the product was created.</param>
    /// <param name="updatedAt">Timestamp of the last update to the product.</param>
    /// <param name="deletedAt">Timestamp when the product was soft-deleted (null if active).</param>
    /// <returns>The initialized or updated product aggregate.</returns>
    public static Product Reconstitute(
        Guid id,
        Rating overallRating,
        long sellerId,
        int subCategoryId,
        ProductStatus status,
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
    }*/