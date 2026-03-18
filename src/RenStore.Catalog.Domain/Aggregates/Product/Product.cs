using RenStore.Catalog.Domain.Aggregates.Product.Events;
using RenStore.Catalog.Domain.Constants;
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
    
    public Guid UpdatedById { get; private set; } 
    public string UpdatedByRole { get; private set; } 
    
    /// <summary>
    /// Unique identifier of the seller.
    /// </summary>
    public Guid SellerId { get; private set; }
    
    /// <summary>
    /// Unique identifier of the sub category.
    /// </summary>
    public Guid SubCategoryId { get; private set; }
    
    /// <summary>
    /// The collection of product variant identifiers associated with this product.
    /// </summary>
    public IReadOnlyCollection<Guid> ProductVariantIds => _productVariantIds.AsReadOnly(); 
    
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
        Guid sellerId,
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
    
    public void MarkAsPublished(
        DateTimeOffset now)
    {
        EndureNotDeleted();

        if (!_productVariantIds.Any())
            throw new DomainException(
                "Product must have variants.");

        if (Status == ProductStatus.Published)
            return;
            
        Raise(new ProductPublishedEvent(
            EventId: Guid.NewGuid(), 
            Status: ProductStatus.Published,
            ProductId: Id,
            OccurredAt: now));
    }
    
    public void MarkAsRejected(
        DateTimeOffset now,
        Guid updatedById,
        string updatedByRole)
    {
        EndureNotDeleted();

        if (updatedById == Guid.Empty)
            throw new DomainException(
                "Updated By ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException(
                "Updated By role cannot be empty string.");
        
        Raise(new ProductRejectedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            Status: ProductStatus.Rejected,
            ProductId: Id,
            OccurredAt: now));
    }
    
    public void MarkAsApproved(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EndureNotDeleted();
        
        if (updatedById == Guid.Empty)
            throw new DomainException(
                "Updated By ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException(
                "Updated By role cannot be empty string.");
        
        Raise(new ProductApprovedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            Status: ProductStatus.Approved,
            ProductId: Id,
            OccurredAt: now));
    }
    
    public void MarkAsDraft(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EndureNotDeleted();
        
        if (updatedById == Guid.Empty)
            throw new DomainException(
                "Updated By ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException(
                "Updated By role cannot be empty string.");
        
        Raise(new ProductMovedToDraftEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            Status: ProductStatus.Draft,
            ProductId: Id,
            OccurredAt: now));
    }
    
    public void MarkAsArchived(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EndureNotDeleted();
        
        if (updatedById == Guid.Empty)
            throw new DomainException(
                "Updated By ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException(
                "Updated By role cannot be empty string.");
        
        Raise(new ProductArchivedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            Status: ProductStatus.Archived,
            ProductId: Id,
            OccurredAt: now));
    }
    
    public void MarkAsHidden(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EndureNotDeleted();
        
        if (updatedById == Guid.Empty)
            throw new DomainException(
                "Updated By ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException(
                "Updated By role cannot be empty string.");
        
        Raise(new ProductHiddenEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            Status: ProductStatus.Hidden,
            ProductId: Id,
            OccurredAt: now));
    }
    
    public void Delete(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EndureNotDeleted();
        
        if (updatedById == Guid.Empty)
            throw new DomainException(
                "Updated By ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException(
                "Updated By role cannot be empty string.");
        
        Raise(new ProductRemovedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
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
    
    public void Restore(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        if(Status != ProductStatus.Deleted)
            throw new DomainException(
                "Cannot restore active entity.");
        
        if (updatedById == Guid.Empty)
            throw new DomainException(
                "Updated By ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException(
                "Updated By role cannot be empty string.");
        
        Raise(new ProductRestoredEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
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
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                Status = e.Status; 
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductApprovedEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                Status = e.Status; 
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductMovedToDraftEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                Status = e.Status; 
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductArchivedEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                Status = e.Status; 
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductHiddenEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                Status = e.Status; 
                UpdatedAt = e.OccurredAt;
                break;
            
            case ProductRemovedEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                Status = e.Status; 
                UpdatedAt = e.OccurredAt;
                DeletedAt = e.OccurredAt;
                break;
            
            case ProductRestoredEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
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
        if (_productVariantIds.Count >= CatalogConstants.Product.MaxVariantsCount)
            throw new DomainException($"Product variants count must be less than {CatalogConstants.Product.MaxVariantsCount}.");

        if (variantId == Guid.Empty)
            throw new DomainException("Product variant Id cannot be empty guid.");
        
        if(_productVariantIds.Contains(variantId))
            throw new DomainException("Product variants reference already exists.");
    }
    
    private static void SellerIdValidate(Guid sellerId)
    {
        if (sellerId == Guid.Empty)
            throw new DomainException("Seller ID cannot be empty guid.");
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