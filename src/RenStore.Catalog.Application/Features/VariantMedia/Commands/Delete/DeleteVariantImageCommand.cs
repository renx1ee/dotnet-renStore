using MediatR;

namespace RenStore.Catalog.Application.Features.VariantMedia.Commands.Delete;

public sealed record DeleteVariantImageCommand(
    Guid VariantId,
    Guid ImageId) 
    : IRequest;