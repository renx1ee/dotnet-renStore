namespace RenStore.Inventory.WebApi.Requests.Stock;

public sealed record FindStockByVariantIdAndSizeIdManageRequest(
    bool? IsDeleted = null);