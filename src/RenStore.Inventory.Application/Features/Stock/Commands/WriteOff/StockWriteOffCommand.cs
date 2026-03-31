using RenStore.Inventory.Domain.Enums;

namespace RenStore.Inventory.Application.Features.Stock.Commands.WriteOff;

public sealed record StockWriteOffCommand(
    Guid StockId,
    WriteOffReason Reason,
    int Count)
    : IRequest;