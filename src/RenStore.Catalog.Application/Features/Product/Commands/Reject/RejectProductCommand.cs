using MediatR;

namespace RenStore.Catalog.Application.Features.Product.Commands.Reject;

public sealed record RejectProductCommand(Guid ProductId) : IRequest;