namespace RenStore.Inventory.WebApi.Requests;

public record FindStockByIdManageRequest(
    bool? IsDeleted = null);