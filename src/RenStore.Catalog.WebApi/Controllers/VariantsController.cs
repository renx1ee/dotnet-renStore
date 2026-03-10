using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.AddPrice;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.AddSize;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.Archive;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.ChangeName;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.Create;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.RemoveSize;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.RestoreSize;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.SoftDelete;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.ToDraft;
using RenStore.Catalog.WebApi.Requests.Variant;

namespace RenStore.Catalog.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("/api/v{version:apiVersion}/catalog")]
public sealed class VariantsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    
    // TODO: restore variant
    [HttpPost("products/{productId:guid}/variants")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Create(
        Guid productId,
        [FromBody] CreateVariantRequest request)
    {
        await _mediator.Send(new CreateProductVariantCommand(
                ProductId: productId,
                ColorId: request.ColorId,
                Name: request.Name,
                SizeSystem: request.SizeSystem,
                SizeType: request.SizeType
                ));
        
        return Created();
    }

    [HttpPatch("variants/{variantId:guid}/archive")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Archive(
        Guid variantId)
    {
        await _mediator.Send(
            new ArchiveProductVariantCommand(variantId));
        
        return NoContent();
    }
    
    [HttpPatch("variants/{variantId:guid}/draft")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Draft(
        Guid variantId)
    {
        await _mediator.Send(
            new DraftProductVariantCommand(variantId));
        
        return NoContent();
    }
    
    [HttpPatch("variants/{variantId:guid}/change-name")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> ChangeName(
        Guid variantId,
        [FromBody] ChangeVariantNameRequest request)
    {
        await _mediator.Send(
            new ChangeProductVariantNameCommand(
                VariantId: variantId,
                Name: request.Name));
        
        return NoContent();
    }
    
    [HttpPost("variants/{variantId:guid}/size")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> AddSize(
        Guid variantId,
        [FromBody] AddSizeToVariantRequest request)
    {
        await _mediator.Send(
            new AddSizeToVariantCommand(
                VariantId: variantId,
                LetterSize: request.LetterSize));
        
        return NoContent();
    }
    
    [HttpPost("variants/{variantId:guid}/size/{sizeId:guid}/price")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> AddPrice(
        Guid variantId,
        Guid sizeId,
        [FromBody] AddPriceToSize request)
    {
        await _mediator.Send(
            new AddPriceToVariantSizeCommand(
                VariantId: variantId,
                SizeId: sizeId,
                Currency: request.Currency,
                ValidFrom: request.ValidFrom,
                Price: request.Price));
        
        return NoContent();
    }
    
    [HttpDelete("variants/{variantId:guid}/size/{sizeId:guid}/remove")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> RemoveSize(
        Guid variantId,
        Guid sizeId)
    {
        await _mediator.Send(
            new RemoveVariantSizeCommand(
                VariantId: variantId,
                SizeId: sizeId));
        
        return NoContent();
    }
    
    [HttpPatch("variants/{variantId:guid}/size/{sizeId:guid}/restore")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> RestoreSize(
        Guid variantId,
        Guid sizeId)
    {
        await _mediator.Send(
            new RestoreVariantSizeCommand(
                VariantId: variantId,
                SizeId: sizeId));
        
        return NoContent();
    }
    
    [HttpDelete("variants/{variantId:guid}/")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> SoftDelete(Guid variantId)
    {
        await _mediator.Send(
            new SoftDeleteProductVariantCommand(
                VariantId: variantId));
        
        return NoContent();
    }
    
    [HttpGet("products/{productId:guid}/variants")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetByProductId(Guid productId)
    {
        return Ok();
    }
    
    [HttpGet("/variants/{variantId:guid}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetById(Guid variantId)
    {
        return Ok();
    }
    
    [HttpGet("variants/{variantId:guid}/sizes/{sizeId:guid}/price-history")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetPriceHistory(
        Guid variantId, 
        Guid sizeId)
    {
        return Ok();
    }
}