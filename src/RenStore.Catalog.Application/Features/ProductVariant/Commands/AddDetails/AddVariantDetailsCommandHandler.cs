namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.AddDetails;

internal sealed class AddVariantDetailsCommandHandler
    : IRequestHandler<AddVariantDetailsCommand>
{
    private readonly ILogger<AddVariantDetailsCommandHandler> _logger;
    private readonly IProductVariantRepository _variantRepository;
    
    public AddVariantDetailsCommandHandler(
        ILogger<AddVariantDetailsCommandHandler> logger,
        IProductVariantRepository variantRepository)
    {
        _logger = logger;
        _variantRepository = variantRepository;
    }
    
    public async Task Handle(
        AddVariantDetailsCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with VariantId: {VariantId}",
            nameof(AddVariantDetailsCommand),
            request.VariantId);
        
        var variant = await _variantRepository
            .GetAsync(request.VariantId, cancellationToken);
        
        if (variant is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Variant.ProductVariant),
                request.VariantId);
        }
        
        variant.AddDetails(
            now: DateTimeOffset.UtcNow,
            countryOfManufactureId: request.CountryOfManufactureId,
            description: request.Description,
            composition: request.Composition,
            caringOfThings: request.CaringOfThings,
            typeOfPacking: request.TypeOfPacking,
            modelFeatures: request.ModelFeatures,
            decorativeElements: request.DecorativeElements,
            equipment: request.Equipment);
        
        await _variantRepository.SaveAsync(variant, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. VariantId: {VariantId}",
            nameof(AddVariantDetailsCommand),
            request.VariantId);
    }
}