namespace RenStore.Catalog.Domain.DomainService;

public class PublishService
{
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
    /*public void Publish(DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        if (!_sizes.Any())
            throw new DomainException("Variant must have sizes.");
        
        if (!_images.Any())
            throw new DomainException("Variant must have images.");
        
        if (_details == null)
            throw new DomainException("Variant must have details.");
        
        Raise(new VariantPublished(
            StockId: Id,
            OccurredAt: now));
    }*/
}