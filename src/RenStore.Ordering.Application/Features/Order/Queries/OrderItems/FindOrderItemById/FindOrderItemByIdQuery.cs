using MediatR;
using RenStore.Order.Domain.ReadModels;

namespace RenStore.Order.Application.Features.Order.Queries.OrderItems.FindOrderItemById;

public sealed record FindOrderItemByIdQuery(
    Guid OrderItemId) 
    : IRequest<OrderItemReadModel?>;