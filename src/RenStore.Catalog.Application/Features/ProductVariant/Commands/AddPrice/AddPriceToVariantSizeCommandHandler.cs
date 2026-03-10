using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Catalog.Domain.Interfaces.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.AddPrice;

public class AddPriceToVariantSizeCommandHandler
    : IRequestHandler<AddPriceToVariantSizeCommand>
{
    private readonly ILogger<AddPriceToVariantSizeCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    
    public AddPriceToVariantSizeCommandHandler(
        ILogger<AddPriceToVariantSizeCommandHandler> logger,
        IProductVariantRepository variantRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
    }
    
    public async Task Handle(
        AddPriceToVariantSizeCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId} and SizeId: {SizeId}",
            nameof(AddPriceToVariantSizeCommand),
            request.VariantId,
            request.SizeId);

        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken);
        
        if(variant is null)
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);

        variant.AddPriceToSize(
            amount: request.Price,
            currency: request.Currency,
            validFrom: request.ValidFrom,
            now: DateTimeOffset.UtcNow,
            sizeId: request.SizeId);

        await _variantRepository.SaveAsync(variant, cancellationToken);

        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}, SizeId: {SizeId}",
            nameof(AddPriceToVariantSizeCommand),
            request.VariantId,
            request.SizeId);
    }
}