using MediatR;
using RenStore.Order.Domain.ReadModels;

namespace RenStore.Order.Application.Features.Order.Queries.Orders.FindById;

public sealed record FindOrderByIdQuery(
    Guid OrderId) 
    : IRequest<OrderReadModel?>;