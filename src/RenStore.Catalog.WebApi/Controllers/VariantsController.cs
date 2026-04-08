using RenStore.Catalog.Application.Features.ProductVariant.Commands.AddAttribute;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.AddDetails;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.Restore;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.RestoreAttribute;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.SoftDeleteAttribute;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.SoftDeleteSize;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.UpdateAttribute;
using RenStore.Catalog.Application.Features.ProductVariant.Commands.UpdateDetails;
using RenStore.Catalog.Application.Features.ProductVariant.Queries.SearchVariants;

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
    /*[Authorize(Roles = Roles.Seller)]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> Create(
        [FromRoute] Guid productId,
        [FromBody] CreateVariantRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(
            new CreateProductVariantCommand(
                ProductId: productId,
                ColorId: request.ColorId,
                Name: request.Name,
                SizeSystem: request.SizeSystem,
                SizeType: request.SizeType
                ), 
            cancellationToken);
        
        return CreatedAtAction(
            actionName: nameof(FindByUrlSlug),
            routeValues: new { urlSlug = response.UrlSlug },
            value: response);
    }
    
    [HttpDelete("manage/variants/{variantId:guid}/")]
    /*[Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> SoftDelete(
        [FromRoute] Guid variantId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new SoftDeleteProductVariantCommand(
                VariantId: variantId), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}")]
    /*[Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> Restore(
        [FromRoute] Guid variantId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new RestoreVariantCommand(
                VariantId: variantId), 
            cancellationToken);
        
        return NoContent();
    }

    [HttpPatch("manage/variants/{variantId:guid}/publish")]
    /*[Authorize(Roles = Roles.Seller)]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> Publish(
        [FromRoute] Guid variantId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new PublishProductVariantCommand(
                VariantId: variantId), 
            cancellationToken);
        
        return NoContent();
    }  
    
    [HttpPatch("manage/variants/{variantId:guid}/{imageId:guid}/set-main-image-id")]
    /*[Authorize(Roles = Roles.Seller)]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> SetMainImage(
        [FromRoute] Guid variantId,
        [FromRoute] Guid imageId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new SetVariantMainImageCommand(
                VariantId: variantId,
                ImageId: imageId), 
            cancellationToken);
        
        return NoContent();
    } 
    
    [HttpPatch("manage/variants/{variantId:guid}/archive")]
    /*[Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> Archive(
        [FromRoute] Guid variantId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ArchiveProductVariantCommand(
                VariantId: variantId), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/draft")]
    /*[Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> Draft(
        [FromRoute] Guid variantId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DraftProductVariantCommand(
                VariantId: variantId), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/change-name")]
    /*[Authorize(Roles = Roles.Seller)]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> ChangeName(
        [FromRoute] Guid variantId,
        [FromBody] ChangeVariantNameRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ChangeProductVariantNameCommand(
                VariantId: variantId,
                Name: request.Name),
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPut("manage/variants/{variantId:guid}/details")]
    /*[Authorize(Roles = Roles.Seller)]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> AddDetails(
        [FromRoute] Guid variantId,
        [FromBody] AddVariantDetailsRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new AddVariantDetailsCommand(
                VariantId: variantId,
                CountryOfManufacture: request.CountryOfManufacture,
                Description: request.Description,
                Composition: request.Composition,
                ModelFeatures: request.ModelFeatures,
                DecorativeElements: request.DecorativeElements,
                Equipment: request.Equipment,
                CaringOfThings: request.CaringOfThings,
                TypeOfPacking: request.TypeOfPacking), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/update-details")]
    /*[Authorize(Roles = Roles.Seller)]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateDetails(
        [FromRoute] Guid variantId,
        [FromBody] UpdateDetailsRequest request,
        CancellationToken cancellationToken)
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
                TypeOfPacking: request.TypeOfPacking), cancellationToken);
        
        return NoContent();
    }
    
    [HttpPost("manage/variants/{variantId:guid}/size")]
    /*[Authorize(Roles = Roles.Seller)]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> AddSize(
        [FromRoute] Guid variantId,
        [FromBody] AddSizeToVariantRequest request,
        CancellationToken cancellationToken)
    {
        var sizeId = await _mediator.Send(
            new AddSizeToVariantCommand(
                VariantId: variantId,
                LetterSize: request.LetterSize), 
            cancellationToken);

        return Ok(new 
        {
            Id = sizeId,
            VariantId = variantId
        });
        
        // TODO: сделать поиск размера
        return CreatedAtAction(
            actionName: nameof(FindById),
            routeValues: new { sizeId },
            value: new
            {
                Id = sizeId,
                VariantId = variantId
            });
    }
    
    [HttpPost("manage/variants/{variantId:guid}/size/{sizeId:guid}/price")]
    /*[Authorize(Roles = Roles.Seller)]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> AddPrice(
        [FromRoute] Guid variantId,
        [FromRoute] Guid sizeId,
        [FromBody] AddPriceToSize request,
        CancellationToken cancellationToken)
    {
        var priceId = await _mediator.Send(
            new AddPriceToVariantSizeCommand(
                VariantId: variantId,
                SizeId: sizeId,
                Currency: request.Currency,
                ValidFrom: request.ValidFrom,
                Price: request.Price), 
            cancellationToken);

        return CreatedAtAction(
            actionName: nameof(FindById),
            routeValues: new { VariantId = variantId },
            value: new
            {
                Id = priceId, 
                SizeId = sizeId, 
                VariantId = variantId
            });
    }
    
    [HttpDelete("manage/variants/{variantId:guid}/size/{sizeId:guid}/remove")]
    /*[Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> RemoveSize(
        [FromRoute] Guid variantId,
        [FromRoute] Guid sizeId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new SoftDeleteVariantSizeCommand(
                VariantId: variantId,
                SizeId: sizeId), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/size/{sizeId:guid}/restore")]
    /*[Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> RestoreSize(
        [FromRoute] Guid variantId,
        [FromRoute] Guid sizeId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new RestoreVariantSizeCommand(
                VariantId: variantId,
                SizeId: sizeId), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPost("manage/variants/{variantId:guid}/attribute")]
    /*[Authorize(Roles = Roles.Seller)]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> AddAttribute(
        [FromRoute] Guid variantId,
        [FromBody] AddAttributeToVariantRequest request,
        CancellationToken cancellationToken)
    {
        var attributeId = await _mediator.Send(
            new AddAttributeToVariantCommand(
                VariantId: variantId,
                Key: request.Key,
                Value: request.Value),
            cancellationToken);
        
        return CreatedAtAction(
            actionName: nameof(FindById),
            routeValues: new { Id = attributeId },
            value: new
            {
                Id = attributeId, 
                VariantId = variantId
            });
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/attributes/{attributeId:guid}/update")]
    /*[Authorize(Roles = Roles.Seller)]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateAttribute(
        [FromRoute] Guid variantId,
        [FromRoute] Guid attributeId,
        [FromBody] UpdateAttributeRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new UpdateVariantAttributeCommand(
                VariantId: variantId,
                AttributeId: attributeId,
                Key: request.Key,
                Value: request.Value), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpDelete("manage/variants/{variantId:guid}/attributes/{attributeId:guid}/remove")]
    /*[Authorize(Roles = Roles.Seller)]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> RemoveAttribute(
        [FromRoute] Guid variantId,
        [FromRoute] Guid attributeId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new SoftDeleteAttributeFromVariantCommand(
                VariantId: variantId,
                AttributeId: attributeId), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/attributes/{attributeId:guid}/restore")]
    /*[Authorize(Roles = Roles.Seller)]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> RestoreAttribute(
        [FromRoute] Guid variantId,
        [FromRoute] Guid attributeId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new RestoreAttributeFromVariantCommand(
                VariantId: variantId,
                AttributeId: attributeId), 
            cancellationToken);
        
        return NoContent();
    }
    
    #endregion
    
    #region Queries Manage
    
    [HttpGet("manage/variants/{variantId:guid}")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByIdManage(
        [FromRoute] Guid variantId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindVariantByIdQuery(
                VariantId: variantId),
            cancellationToken);
        
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("manage/variants/{article:long}")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByArticleManage(
        [FromRoute] long article,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindVariantByArticleQuery(Article: article), 
            cancellationToken);
        
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("manage/products/{productId:guid}/variants")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByProductIdManage(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken,
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
                IsDeleted: isDeleted), 
            cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("manage/variants/{variantId:guid}/sizes/{sizeId:guid}/price-history")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindPriceHistoryManage(
        [FromRoute] Guid variantId, 
        [FromRoute] Guid sizeId,
        CancellationToken cancellationToken,
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
                IsActive: isActive), 
            cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("manage/variants/{variantId:guid}/sizes")]
    [Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindSizesByVariantIdManage(
        [FromRoute] Guid variantId,
        CancellationToken cancellationToken,
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
                IsDeleted: isDeleted), 
            cancellationToken);
        
        return Ok(result);
    }
    
    #endregion
    
    #region Queries Catalog
    
    [HttpGet("catalog/variants/{variantId:guid}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindById(
        [FromRoute] Guid variantId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindVariantByIdQuery(variantId), 
            cancellationToken);
        
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("catalog/variants/{article:long}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByArticle(
        [FromRoute] long article,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindVariantByArticleQuery(Article: article), 
            cancellationToken);
        
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("catalog/product/{productId:guid}/variants")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByProductId(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken,
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
                Descending: descending), 
            cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("catalog/variants/{variantId:guid}/sizes/{sizeId:guid}/price-history")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindPriceHistory(
        [FromRoute] Guid variantId, 
        [FromRoute] Guid sizeId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindPriceHistoryBySizeIdQuery(
                VariantId: variantId,
                SizeId: sizeId,
                SortBy: PriceHistorySortBy.CreatedAt,
                Descending: true), 
            cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("catalog/variants/{variantId:guid}/sizes")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindSizesByVariantId(
        [FromRoute] Guid variantId, 
        CancellationToken cancellationToken,
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
                IsDeleted: false), 
            cancellationToken);
        
        return Ok(result);
    }
    // TODO:
    [HttpGet("catalog/variants/{urlSlug}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByUrlSlug(
        [FromRoute] string urlSlug, 
        CancellationToken cancellationToken)
    {
        /*var result = await _mediator.Send(
            new FindSizesByVariantIdQuery(
                VariantId: variantId,
                SortBy: sortBy,
                Page: page,
                PageCount: pageCount,
                Descending: descending,
                IsDeletedCategory: false), 
            cancellationToken);
        
        return Ok(result);*/

        return NoContent();
    }
    
    [HttpGet("catalog/variants")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Search(
        [FromQuery] SearchVariantsRequest request, 
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new SearchVariantsQuery(
                Page: request.Page,
                PageSize: request.PageSize,
                Descending: request.Descending,
                CategoryId: request.CategoryId,
                SubCategoryId: request.SubCategoryId,
                MaxPrice: request.MaxPrice,
                MinPrice: request.MinPrice,
                ColorId: request.ColorId,
                Search: request.Search,
                SortBy: request.SortBy), 
            cancellationToken);

        return Ok(result);
    }
    
    #endregion
}