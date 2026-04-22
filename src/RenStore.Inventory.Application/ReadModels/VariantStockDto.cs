namespace RenStore.Inventory.Application.ReadModels;

public sealed record VariantStockDto
(
    Guid      Id,
    int       InStock,
    int       Sales,
    string?   WriteOffReason,
    DateTime  CreatedAt,
    DateTime? UpdatedAt,
    DateTime? DeletedAt,
    bool      IsDeleted,
    string?   UpdatedById,
    string?   UpdatedByRole,
    Guid      VariantId,
    Guid      SizeId
);