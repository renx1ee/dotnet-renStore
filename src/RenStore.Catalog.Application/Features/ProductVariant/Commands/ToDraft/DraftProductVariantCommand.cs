using MediatR;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.ToDraft;

public sealed record DraftProductVariantCommand(Guid VariantId) : IRequest;