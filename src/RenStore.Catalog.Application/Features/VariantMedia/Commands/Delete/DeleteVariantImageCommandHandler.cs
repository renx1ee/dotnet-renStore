using RenStore.Catalog.Domain.Aggregates.Media;

namespace RenStore.Catalog.Application.Features.VariantMedia.Commands.Delete;

internal sealed class DeleteVariantImageCommandHandler
    : IRequestHandler<DeleteVariantImageCommand>
{
    private readonly ILogger<DeleteVariantImageCommandHandler> _logger;
    private readonly IVariantImageRepository _variantImageRepository;
    
    public DeleteVariantImageCommandHandler(
        ILogger<DeleteVariantImageCommandHandler> logger,
        IVariantImageRepository variantImageRepository)
    {
        _variantImageRepository = variantImageRepository;
        _logger = logger;
    }
    
    public async Task Handle(
        DeleteVariantImageCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ImageId: {ImageId}",
            nameof(DeleteVariantImageCommand),
            request.ImageId);
        
        var image = await _variantImageRepository.GetAsync(
            id: request.ImageId,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException(
                name: typeof(VariantImage),
                request.ImageId);

        if (image.IsMain)
            throw new DomainException("Cannot delete main image. Set another image as main first.");
        
        image.Delete(DateTimeOffset.UtcNow);

        await _variantImageRepository.SaveAsync(image, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. ImageId: {ImageId}",
            nameof(DeleteVariantImageCommand),
            request.ImageId);
    }
}