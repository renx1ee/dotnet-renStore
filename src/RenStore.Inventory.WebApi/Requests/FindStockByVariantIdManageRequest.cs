namespace RenStore.Inventory.WebApi.Requests;

public sealed record FindStockByVariantIdManageRequest(
    bool? IsDeleted = null);

public sealed record FindStockByVariantIdAndSizeIdManageRequest(
    bool? IsDeleted = null);