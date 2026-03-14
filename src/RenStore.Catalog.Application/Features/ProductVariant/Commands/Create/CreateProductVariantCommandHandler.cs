using MediatR;
using Microsoft.Extensions.Logging;
using RenStore.Catalog.Application.Abstractions.Projections;
using RenStore.Catalog.Domain.Interfaces.Repository;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Application.Features.ProductVariant.Commands.Create;

internal sealed class CreateProductVariantCommandHandler
    : IRequestHandler<CreateProductVariantCommand>
{
    private readonly ILogger<CreateProductVariantCommandHandler> _logger;
    private readonly IProductVariantRepository _productVariantRepository;
    private readonly IProductRepository _productRepository;
    
    public CreateProductVariantCommandHandler(
        ILogger<CreateProductVariantCommandHandler> logger,
        IProductRepository productRepository,
        IProductVariantRepository productVariantRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
        _productVariantRepository = productVariantRepository;
    }

    public async Task Handle(
        CreateProductVariantCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {Command} with ProductId: {ProductId}",
            nameof(CreateProductVariantCommand),
            request.ProductId);
        
        // TODO: сделать проверки
        /*var belong = await _productProjection
            .BelongAsync(
                productId: request.ProductId,
                sellerId: sellerId,
                cancellationToken: cancellationToken);
        var existingProduct = await _productProjection
               .ExistsAsync(request.ProductId, cancellationToken);
           
           if (!existingProduct)
           {
               throw new NotFoundException(
                   name: typeof(Domain.Aggregates.Product.Product),
                   request.ProductId);
           }
        */
        
        var product = await _productRepository
            .GetAsync(request.ProductId, cancellationToken);
        
        if (product is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Product.Product),
                request.ProductId);
        }
        
        Random rnd = new Random();
        var article = rnd.Next(1, 100000000); // TODO: сделать сервис для генерации артикула
        var url = Guid.NewGuid().ToString();      // TODO: сделать сервис для url

        var now = DateTimeOffset.UtcNow;

        var variant = Domain.Aggregates.Variant.ProductVariant
            .Create(
                now: now,
                productId: request.ProductId,
                colorId: request.ColorId,
                name: request.Name,
                sizeSystem: request.SizeSystem,
                sizeType: request.SizeType,
                article: article,
                url: url);
        
        product.AddVariantReference(variant.Id, now);

        await _productVariantRepository.SaveAsync(variant, cancellationToken);
        
        await _productRepository.SaveAsync(product, cancellationToken);
        
        _logger.LogInformation(
            "{Command} handled. ProductId: {ProductId}",
            nameof(CreateProductVariantCommand),
            request.ProductId);
    }
}