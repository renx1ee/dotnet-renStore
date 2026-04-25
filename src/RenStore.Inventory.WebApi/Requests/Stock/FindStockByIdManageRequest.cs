namespace RenStore.Inventory.WebApi.Requests.Stock;

public record FindStockByIdManageRequest(
    bool? IsDeleted = null);