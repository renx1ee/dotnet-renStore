using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Catalog.Domain.Interfaces.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.AddSize;

internal sealed class AddSizeToVariantCommandHandler
    : IRequestHandler<AddSizeToVariantCommand>
{
    private readonly ILogger<AddSizeToVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    
    public AddSizeToVariantCommandHandler(
        ILogger<AddSizeToVariantCommandHandler> logger,
        IProductVariantRepository variantRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
    }
    
    public async Task Handle(
        AddSizeToVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(AddSizeToVariantCommand),
            request.VariantId);

        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);

        variant.AddSize(
            letterSize: request.LetterSize,
            now: DateTimeOffset.UtcNow);

        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(AddSizeToVariantCommand),
            request.VariantId);
    }
}