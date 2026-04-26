using MediatR;
using RenStore.Order.Domain.ReadModels;

namespace RenStore.Order.Application.Features.Order.Queries.Orders.FindByOrderById;

public sealed record FindMyOrderByIdQuery(
    Guid OrderId) 
    : IRequest<OrderReadModel?>;