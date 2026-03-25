using RenStore.Catalog.Application.Features.ProductVariant.Commands.AddAttribute;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.DetailsUpdate;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.Restore;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.RestoreAttribute;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.SoftDeleteAttribute;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.UpdateAttribute;

namespace RenStore.Catalog.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("/api/v{version:apiVersion}")]
public sealed class VariantsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    
    #region Commands
    
    // TODO: restore variant
    [HttpPost("manage/products/{productId:guid}/variants")]
    [Authorize(Roles = Roles.Seller)]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Create(
        [FromRoute] Guid productId,
        [FromBody] CreateVariantRequest request)
    {
        var variantId = await _mediator.Send(
            new CreateProductVariantCommand(
                ProductId: productId,
                ColorId: request.ColorId,
                Name: request.Name,
                SizeSystem: request.SizeSystem,
                SizeType: request.SizeType
                ));
        
        return CreatedAtAction(
            actionName: nameof(FindById),
            routeValues: new { variantId, version = "1" },
            value: new { Id = variantId });
    }
    
    [HttpDelete("manage/variants/{variantId:guid}/")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> SoftDelete(
        [FromRoute] Guid variantId)
    {
        await _mediator.Send(
            new SoftDeleteProductVariantCommand(
                VariantId: variantId));
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Restore(
        [FromRoute] Guid variantId)
    {
        await _mediator.Send(
            new RestoreVariantCommand(
                VariantId: variantId));
        
        return NoContent();
    }

    [HttpPatch("manage/variants/{variantId:guid}/publish")]
    [Authorize(Roles = Roles.Seller)]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Publish(
        [FromRoute] Guid variantId)
    {
        await _mediator.Send(
            new PublishProductVariantCommand(
                VariantId: variantId));
        
        return NoContent();
    }  
    
    [HttpPatch("manage/variants/{variantId:guid}/{imageId:guid}/set-main-image-id")]
    [Authorize(Roles = Roles.Seller)]
    [MapToApiVersion(1)]
    public async Task<IActionResult> SetMainImage(
        [FromRoute] Guid variantId,
        [FromRoute] Guid imageId)
    {
        await _mediator.Send(
            new SetVariantMainImageCommand(
                VariantId: variantId,
                ImageId: imageId));
        
        return NoContent();
    } 
    
    [HttpPatch("manage/variants/{variantId:guid}/archive")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Archive(
        [FromRoute] Guid variantId)
    {
        await _mediator.Send(
            new ArchiveProductVariantCommand(
                VariantId: variantId));
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/draft")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Draft(
        [FromRoute] Guid variantId)
    {
        await _mediator.Send(
            new DraftProductVariantCommand(
                VariantId: variantId));
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/change-name")]
    [Authorize(Roles = Roles.Seller)]
    [MapToApiVersion(1)]
    public async Task<IActionResult> ChangeName(
        [FromRoute] Guid variantId,
        [FromBody] ChangeVariantNameRequest request)
    {
        await _mediator.Send(
            new ChangeProductVariantNameCommand(
                VariantId: variantId,
                Name: request.Name));
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/update-details")]
    [Authorize(Roles = Roles.Seller)]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateDetails(
        [FromRoute] Guid variantId,
        [FromBody] UpdateDetailsRequest request)
    {
        await _mediator.Send(
            new UpdateVariantDetailsCommand(
                VariantId: variantId,
                Description: request.Description,
                Composition: request.Composition,
                ModelFeatures: request.ModelFeatures,
                DecorativeElements: request.DecorativeElements,
                Equipment: request.Equipment,
                CaringOfThings: request.CaringOfThings,
                TypeOfPacking: request.TypeOfPacking));
        
        return NoContent();
    }
    
    [HttpPost("manage/variants/{variantId:guid}/size")]
    [Authorize(Roles = Roles.Seller)]
    [MapToApiVersion(1)]
    public async Task<IActionResult> AddSize(
        [FromRoute] Guid variantId,
        [FromBody] AddSizeToVariantRequest request)
    {
        var sizeId = await _mediator.Send(
            new AddSizeToVariantCommand(
                VariantId: variantId,
                LetterSize: request.LetterSize));
        
        return CreatedAtAction(
            actionName: nameof(FindById),
            routeValues: new { sizeId, version = "1" },
            value: new
            {
                Id = sizeId,
                VariantId = variantId
            });
    }
    
    [HttpPost("manage/variants/{variantId:guid}/size/{sizeId:guid}/price")]
    [Authorize(Roles = Roles.Seller)]
    [MapToApiVersion(1)]
    public async Task<IActionResult> AddPrice(
        [FromRoute] Guid variantId,
        [FromRoute] Guid sizeId,
        [FromBody] AddPriceToSize request)
    {
        var priceId = await _mediator.Send(
            new AddPriceToVariantSizeCommand(
                VariantId: variantId,
                SizeId: sizeId,
                Currency: request.Currency,
                ValidFrom: request.ValidFrom,
                Price: request.Price));

        return CreatedAtAction(
            actionName: nameof(FindById),
            routeValues: new { priceId, version = "1" },
            value: new
            {
                Id = priceId, 
                SizeId = sizeId, 
                VariantId = variantId
            });
    }
    
    [HttpDelete("manage/variants/{variantId:guid}/size/{sizeId:guid}/remove")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> RemoveSize(
        [FromRoute] Guid variantId,
        [FromRoute] Guid sizeId)
    {
        await _mediator.Send(
            new SoftDeleteVariantSizeCommand(
                VariantId: variantId,
                SizeId: sizeId));
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/size/{sizeId:guid}/restore")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> RestoreSize(
        [FromRoute] Guid variantId,
        [FromRoute] Guid sizeId)
    {
        await _mediator.Send(
            new RestoreVariantSizeCommand(
                VariantId: variantId,
                SizeId: sizeId));
        
        return NoContent();
    }
    
    [HttpPost("manage/variants/{variantId:guid}/attribute")]
    [Authorize(Roles = Roles.Seller)]
    [MapToApiVersion(1)]
    public async Task<IActionResult> AddAttribute(
        [FromRoute] Guid variantId,
        [FromBody] AddAttributeToVariantRequest request)
    {
        var attributeId = await _mediator.Send(
            new AddAttributeToVariantCommand(
                VariantId: variantId,
                Key: request.Key,
                Value: request.Value));
        
        return CreatedAtAction(
            actionName: nameof(FindById),
            routeValues: new { Id = attributeId, version = "1" },
            value: new
            {
                Id = attributeId, 
                VariantId = variantId
            });
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/attributes/{attributeId:guid}/update")]
    [Authorize(Roles = Roles.Seller)]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateAttribute(
        [FromRoute] Guid variantId,
        [FromRoute] Guid attributeId,
        [FromBody] UpdateAttributeRequest request)
    {
        await _mediator.Send(
            new UpdateVariantAttributeCommand(
                VariantId: variantId,
                AttributeId: attributeId,
                Key: request.Key,
                Value: request.Value));
        
        return NoContent();
    }
    
    [HttpDelete("manage/variants/{variantId:guid}/attributes/{attributeId:guid}/remove")]
    [Authorize(Roles = Roles.Seller)]
    [MapToApiVersion(1)]
    public async Task<IActionResult> RemoveAttribute(
        [FromRoute] Guid variantId,
        [FromRoute] Guid attributeId)
    {
        await _mediator.Send(
            new SoftDeleteAttributeFromVariantCommand(
                VariantId: variantId,
                AttributeId: attributeId));
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/attributes/{attributeId:guid}/restore")]
    [Authorize(Roles = Roles.Seller)]
    [MapToApiVersion(1)]
    public async Task<IActionResult> RestoreAttribute(
        [FromRoute] Guid variantId,
        [FromRoute] Guid attributeId)
    {
        await _mediator.Send(
            new RestoreAttributeFromVariantCommand(
                VariantId: variantId,
                AttributeId: attributeId));
        
        return NoContent();
    }
    
    #endregion
    
    #region Queries Manage
    
    [HttpGet("manage/variants/{variantId:guid}")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByIdManage(
        [FromRoute] Guid variantId)
    {
        var result = await _mediator.Send(
            new FindVariantByIdQuery(
                VariantId: variantId));
        
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("manage/variants/{article:long}")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByArticleManage(
        [FromRoute] long article)
    {
        var result = await _mediator.Send(
            new FindVariantByArticleQuery(
                Article: article));
        
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("manage/products/{productId:guid}/variants")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByProductIdManage(
        [FromRoute] Guid productId,
        [FromQuery] ProductVariantSortBy sortBy = ProductVariantSortBy.Id,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageCount = 25,
        [FromQuery] bool descending = false,
        [FromQuery] bool? isDeleted = null)
    {
        var result = await _mediator.Send(
            new FindVariantsByProductIdQuery(
                SortBy: sortBy,
                ProductId: productId,
                Page: page,
                PageCount: pageCount,
                Descending: descending,
                IsDeleted: isDeleted));
        
        return Ok(result);
    }
    
    [HttpGet("manage/variants/{variantId:guid}/sizes/{sizeId:guid}/price-history")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindPriceHistoryManage(
        [FromRoute] Guid variantId, 
        [FromRoute] Guid sizeId,
        [FromQuery] PriceHistorySortBy sortBy = PriceHistorySortBy.Id,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageCount = 25,
        [FromQuery] bool descending = false,
        [FromQuery] bool? isActive = null)
    {
        var result = await _mediator.Send(
            new FindPriceHistoryBySizeIdQuery(
                VariantId: variantId,
                SortBy: sortBy,
                SizeId: sizeId,
                Page: page,
                PageCount: pageCount,
                Descending: descending,
                IsActive: isActive));
        
        return Ok(result);
    }
    
    [HttpGet("manage/variants/{variantId:guid}/sizes")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindSizesByVariantIdManage(
        [FromRoute] Guid variantId, 
        [FromQuery] VariantSizeSortBy sortBy = VariantSizeSortBy.Id,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageCount = 25,
        [FromQuery] bool descending = false,
        [FromQuery] bool? isDeleted = null)
    {
        var result = await _mediator.Send(
            new FindSizesByVariantIdQuery(
                VariantId: variantId,
                SortBy: sortBy,
                Page: page,
                PageCount: pageCount,
                Descending: descending,
                IsDeleted: isDeleted));
        
        return Ok(result);
    }
    
    #endregion
    
    #region Queries Catalog
    
    [HttpGet("catalog/variants/{variantId:guid}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindById(
        [FromRoute] Guid variantId)
    {
        var result = await _mediator.Send(
            new FindVariantByIdQuery(variantId));
        
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("catalog/variants/{article:long}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByArticle(
        [FromRoute] long article)
    {
        var result = await _mediator.Send(
            new FindVariantByArticleQuery(Article: article));
        
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("catalog/product/{productId:guid}/variants")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByProductId(
        [FromRoute] Guid productId,
        [FromQuery] ProductVariantSortBy sortBy = ProductVariantSortBy.Id,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageCount = 25,
        [FromQuery] bool descending = false)
    {
        var result = await _mediator.Send(
            new FindVariantsByProductIdQuery(
                ProductId: productId,
                SortBy: sortBy,
                Page: page,
                PageCount: pageCount,
                Descending: descending));
        
        return Ok(result);
    }
    
    [HttpGet("catalog/variants/{variantId:guid}/sizes/{sizeId:guid}/price-history")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindPriceHistory(
        [FromRoute] Guid variantId, 
        [FromRoute] Guid sizeId)
    {
        var result = await _mediator.Send(
            new FindPriceHistoryBySizeIdQuery(
                VariantId: variantId,
                SizeId: sizeId,
                SortBy: PriceHistorySortBy.CreatedAt,
                Descending: true));
        
        return Ok(result);
    }
    
    [HttpGet("catalog/variants/{variantId:guid}/sizes")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindSizesByVariantId(
        [FromRoute] Guid variantId, 
        [FromQuery] VariantSizeSortBy sortBy = VariantSizeSortBy.Id,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageCount = 25,
        [FromQuery] bool descending = false)
    {
        var result = await _mediator.Send(
            new FindSizesByVariantIdQuery(
                VariantId: variantId,
                SortBy: sortBy,
                Page: page,
                PageCount: pageCount,
                Descending: descending,
                IsDeleted: false));
        
        return Ok(result);
    }
    
    #endregion
}