namespace RenStore.Inventory.WebApi.Requests.Stock;

public sealed record FindStockByVariantIdManageRequest(
    bool? IsDeleted = null);