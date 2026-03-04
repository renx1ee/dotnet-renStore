using MediatR;

namespace RenStore.Catalog.Application.Features.Product.Commands.Approve;

public sealed record ApproveProductCommand(Guid ProductId) : IRequest;