namespace RenStore.Catalog.Domain.Entities;

public class ProductPriceHistoryEntity
{
    public Guid Id { get; private set; }
    public decimal Price { get; private set; }
    public decimal OldPrice { get; private set; }
    public decimal DiscountPrice { get; private set; }
    public decimal DiscountPercent { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string ChangedBy { get; private set; } = string.Empty;
    public Guid ProductVariantId { get; private set; }
    public ProductVariant? ProductVariant { get; private set; }
}