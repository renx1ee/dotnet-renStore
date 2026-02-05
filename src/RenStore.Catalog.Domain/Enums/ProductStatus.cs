namespace RenStore.Catalog.Domain.Enums;

/// <summary>
/// Represent statuses for product lifecycle.
/// </summary>
public enum ProductStatus
{
    /// <summary>
    /// The product has been created but has not passed moderation.
    /// </summary>
    PendingModeration,
    
    /// <summary>
    /// The product has not passed moderation.
    /// </summary>
    Rejected,
    
    /// <summary>
    /// The product was moderating and ready to publication.
    /// </summary>
    Approved,
    
    /// <summary>
    /// The product in the draft.
    /// </summary>
    Draft,
    
    /// <summary>
    /// The product has been published and presented to customers.
    /// The product available and appears in the catalog.
    /// </summary>
    Published,
    
    /// <summary>
    /// The product has been unpublished and temporarily unavailable to customers.
    /// </summary>
    Hidden,
    
    /// <summary>
    /// The product is obsolete and is no longer sold.
    /// </summary>
    Archived,
    
    /// <summary>
    /// The product was deleted.
    /// </summary>
    IsDeleted
}