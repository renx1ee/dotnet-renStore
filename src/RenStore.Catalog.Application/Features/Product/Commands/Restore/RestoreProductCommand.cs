namespace RenStore.Catalog.Application.Features.Product.Commands.Restore;

public record RestoreProductCommand(Guid ProductId) : IRequest;