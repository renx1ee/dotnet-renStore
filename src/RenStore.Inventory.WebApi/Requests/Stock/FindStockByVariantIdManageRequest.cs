namespace RenStore.Inventory.WebApi.Requests.Stock;

public sealed record FindStockByVariantIdManageRequest(
    bool? IsDeleted = null);

public sealed record FindStockByVariantIdAndSizeIdManageRequest(
    bool? IsDeleted = null);