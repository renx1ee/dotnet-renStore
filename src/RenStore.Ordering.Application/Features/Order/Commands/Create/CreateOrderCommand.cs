using MediatR;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Order.Application.Features.Order.Commands.Create;

public sealed record CreateOrderCommand(
    Guid    CorrelationId,
    Guid    CustomerId,
    Guid    VariantId,
    Guid    SizeId,
    int     Quantity,
    decimal PriceAmount,
    string  Currency,
    string  ProductNameSnapshot,
    string  ShippingAddress) 
    : IRequest<Guid>;