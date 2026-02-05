using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.Aggregates.Product.Events;

/// <summary>
/// The event occurred when the product has been created.
/// </summary>
/// <param name="ProductId">Unique product ID.</param>
/// <param name="SellerId">Unique seller ID.</param>
/// <param name="SubCategoryId">Unique sub category ID.</param>
/// <param name="Status">The product at the time of creation.</param>
/// <param name="OccurredAt">Time of occurrence of the event.</param>
public record ProductCreated(
    Guid ProductId,
    long SellerId,
    int SubCategoryId,
    ProductStatus Status,
    DateTimeOffset OccurredAt);
   
/// <summary>
/// The event occurred when the product has been published.
/// </summary>
/// <param name="ProductId">Unique product ID.</param>
/// <param name="OccurredAt">Time of occurrence of the event.</param>
public record ProductPublished(
    Guid ProductId,
    DateTimeOffset OccurredAt);

/// <summary>
/// The event occurred when the product has been rejected.
/// </summary>
/// <param name="ProductId">Unique product ID.</param>
/// <param name="OccurredAt">Time of occurrence of the event.</param>
public record ProductRejected(
    Guid ProductId,
    DateTimeOffset OccurredAt);

/// <summary>
/// The event occurred when the product has been approved.
/// </summary>
/// <param name="ProductId">Unique product ID.</param>
/// <param name="OccurredAt">Time of occurrence of the event.</param>
public record ProductApproved(
    Guid ProductId,
    DateTimeOffset OccurredAt);

/// <summary>
/// The event occurred when the product has been moved to draft.
/// </summary>
/// <param name="ProductId">Unique product ID.</param>
/// <param name="OccurredAt">Time of occurrence of the event.</param>
public record ProductMovedToDraft(
    Guid ProductId,
    DateTimeOffset OccurredAt);

/// <summary>
/// The event occurred when the product has been archived.
/// </summary>
/// <param name="ProductId">Unique product ID.</param>
/// <param name="OccurredAt">Time of occurrence of the event.</param>
public record ProductArchived(
    Guid ProductId,
    DateTimeOffset OccurredAt);

/// <summary>
/// The event occurred when the product has been hidden.
/// </summary>
/// <param name="ProductId">Unique product ID.</param>
/// <param name="OccurredAt">Time of occurrence of the event.</param>
public record ProductHidden(
    Guid ProductId,
    DateTimeOffset OccurredAt);

/// <summary>
/// The event occurred when the product has been removed.
/// </summary>
/// <param name="ProductId">Unique product ID.</param>
/// <param name="OccurredAt">Time of occurrence of the event.</param>
public record ProductRemoved(
    Guid ProductId, 
    DateTimeOffset OccurredAt);
 
/// <summary>
/// The event occurred when the product has been restored.
/// </summary>
/// <param name="ProductId">Unique product ID.</param>
/// <param name="OccurredAt">Time of occurrence of the event.</param>
public record ProductRestored(
    Guid ProductId, 
    DateTimeOffset OccurredAt);