using RenStore.Inventory.Domain.Enums;

namespace RenStore.Inventory.WebApi.Requests.Stock;

public sealed record StockWriteOffRequest(
    WriteOffReason Reason,
    int Count);