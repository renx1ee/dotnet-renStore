namespace RenStore.Inventory.Application.ReadModels;

public sealed class VariantStockDto
{
    public Guid      Id             { get; set; }
    public int       InStock        { get; set; }
    public int       Sales          { get; set; }
    public string?   WriteOffReason { get; set; }
    public DateTime  CreatedAt      { get; set; }
    public DateTime? UpdatedAt      { get; set; }
    public DateTime? DeletedAt      { get; set; }
    public bool      IsDeleted      { get; set; }
    public string?   UpdatedById    { get; set; }
    public string?   UpdatedByRole  { get; set; }
    public Guid      VariantId      { get; set; }
    public Guid      SizeId         { get; set; }
    
    public VariantStockDto() { }
}