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
    [Authorize(Roles = "Seller")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Create(
        Guid productId,
        [FromBody] CreateVariantRequest request)
    {
        var variantId = await _mediator.Send(
            new CreateProductVariantCommand(
                UserId: User.GetUserId(),
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

    [HttpPatch("manage/variants/{variantId:guid}/publish")]
    [Authorize(Roles = "Seller")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Publish(
        Guid variantId)
    {
        await _mediator.Send(
            new PublishProductVariantCommand(
                VariantId: variantId,
                UserId: User.GetUserId()));
        
        return NoContent();
    }  
    
    [HttpPatch("manage/variants/{variantId:guid}/{imageId:guid}/set-main-image-id")]
    [Authorize(Roles = "Seller")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> SetMainImage(
        Guid variantId,
        Guid imageId)
    {
        await _mediator.Send(
            new SetVariantMainImageCommand(
                UserId: User.GetUserId(),
                VariantId: variantId,
                ImageId: imageId));
        
        return NoContent();
    } 
    
    [HttpPatch("manage/variants/{variantId:guid}/archive")]
    [Authorize(Roles = "Seller,Admin,Moderator")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Archive(
        Guid variantId)
    {
        await _mediator.Send(
            new ArchiveProductVariantCommand(
                UserId: User.GetUserId(),
                Role: User.GetRole(),
                VariantId: variantId));
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/draft")]
    [Authorize(Roles = "Seller,Admin,Moderator")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Draft(
        Guid variantId)
    {
        await _mediator.Send(
            new DraftProductVariantCommand(
                UserId: User.GetUserId(),
                Role: User.GetRole(),
                VariantId: variantId));
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/change-name")]
    [Authorize(Roles = "Seller")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> ChangeName(
        Guid variantId,
        [FromBody] ChangeVariantNameRequest request)
    {
        await _mediator.Send(
            new ChangeProductVariantNameCommand(
                UserId: User.GetUserId(),
                VariantId: variantId,
                Name: request.Name));
        
        return NoContent();
    }
    
    [HttpPost("manage/variants/{variantId:guid}/size")]
    [Authorize(Roles = "Seller")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> AddSize(
        Guid variantId,
        [FromBody] AddSizeToVariantRequest request)
    {
        await _mediator.Send(
            new AddSizeToVariantCommand(
                VariantId: variantId,
                UserId: User.GetUserId(),
                LetterSize: request.LetterSize));
        
        return NoContent();
    }
    
    [HttpPost("manage/variants/{variantId:guid}/size/{sizeId:guid}/price")]
    [Authorize(Roles = "Seller")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> AddPrice(
        Guid variantId,
        Guid sizeId,
        [FromBody] AddPriceToSize request)
    {
        await _mediator.Send(
            new AddPriceToVariantSizeCommand(
                UserId: User.GetUserId(),
                VariantId: variantId,
                SizeId: sizeId,
                Currency: request.Currency,
                ValidFrom: request.ValidFrom,
                Price: request.Price));
        
        return NoContent();
    }
    
    [HttpDelete("manage/variants/{variantId:guid}/size/{sizeId:guid}/remove")]
    [Authorize(Roles = "Seller,Admin,Moderator")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> RemoveSize(
        Guid variantId,
        Guid sizeId)
    {
        await _mediator.Send(
            new RemoveVariantSizeCommand(
                VariantId: variantId,
                UserId: User.GetUserId(),
                Role: User.GetRole(),
                SizeId: sizeId));
        
        return NoContent();
    }
    
    [HttpPatch("manage/variants/{variantId:guid}/size/{sizeId:guid}/restore")]
    [Authorize(Roles = "Seller,Admin,Moderator")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> RestoreSize(
        Guid variantId,
        Guid sizeId)
    {
        await _mediator.Send(
            new RestoreVariantSizeCommand(
                VariantId: variantId,
                UserId: User.GetUserId(),
                Role: User.GetRole(),
                SizeId: sizeId));
        
        return NoContent();
    }
    
    [HttpDelete("manage/variants/{variantId:guid}/")]
    [Authorize(Roles = "Seller,Admin,Moderator")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> SoftDelete(Guid variantId)
    {
        await _mediator.Send(
            new SoftDeleteProductVariantCommand(
                UserId: User.GetUserId(),
                Role: User.GetRole(),
                VariantId: variantId));
        
        return NoContent();
    }
    
    #endregion

    #region Queries Catalog
    
    [HttpGet("catalog/variants/{variantId:guid}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindById(Guid variantId)
    {
        var result = await _mediator.Send(
            new FindVariantByIdQuery(variantId));
        
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("catalog/variants/{article:long}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByArticle(
        long article)
    {
        var result = await _mediator.Send(
            new FindVariantByArticleQuery(Article: article));
        
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("catalog/product/{productId:guid}/variants")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByProductId(
        Guid productId,
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
        Guid variantId, 
        Guid sizeId)
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
        Guid variantId, 
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
    
    #region Queries Manage
    
    [HttpGet("manage/variants/{variantId:guid}")]
    [Authorize(Roles = "Seller,Moderator,Admin")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByIdManage(
        Guid variantId)
    {
        var result = await _mediator.Send(
            new FindVariantByIdQuery( 
                VariantId: variantId, 
                Role: User.GetRole(),
                UserId: User.GetUserId()));
        
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("manage/variants/{article:long}")]
    [Authorize(Roles = "Seller,Moderator,Admin")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByArticleManage(
        long article)
    {
        var result = await _mediator.Send(
            new FindVariantByArticleQuery(
                Article: article,
                Role: User.GetRole(),
                UserId: User.GetUserId()));
        
        return result is null ? NotFound() : Ok(result);
    }
    
    [HttpGet("manage/products/{productId:guid}/variants")]
    [Authorize(Roles = "Seller,Moderator,Admin")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByProductIdManage(
        Guid productId,
        [FromQuery] ProductVariantSortBy sortBy = ProductVariantSortBy.Id,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageCount = 25,
        [FromQuery] bool descending = false,
        [FromQuery] bool? isDeleted = null)
    {
        var result = await _mediator.Send(
            new FindVariantsByProductIdQuery(
                Role: User.GetRole(),
                UserId: User.GetUserId(),
                SortBy: sortBy,
                ProductId: productId,
                Page: page,
                PageCount: pageCount,
                Descending: descending,
                IsDeleted: isDeleted));
        
        return Ok(result);
    }
    
    [HttpGet("manage/variants/{variantId:guid}/sizes/{sizeId:guid}/price-history")]
    [Authorize(Roles = "Seller,Moderator,Admin")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindPriceHistoryManage(
        Guid variantId, 
        Guid sizeId,
        [FromQuery] PriceHistorySortBy sortBy = PriceHistorySortBy.Id,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageCount = 25,
        [FromQuery] bool descending = false,
        [FromQuery] bool? isActive = null)
    {
        var result = await _mediator.Send(
            new FindPriceHistoryBySizeIdQuery(
                Role: User.GetRole(),
                UserId: User.GetUserId(),
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
    [Authorize(Roles = "Seller,Moderator,Admin")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindSizesByVariantIdManage(
        Guid variantId, 
        [FromQuery] VariantSizeSortBy sortBy = VariantSizeSortBy.Id,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageCount = 25,
        [FromQuery] bool descending = false,
        [FromQuery] bool? isDeleted = null)
    {
        var result = await _mediator.Send(
            new FindSizesByVariantIdQuery(
                Role: User.GetRole(),
                UserId: User.GetUserId(),
                VariantId: variantId,
                SortBy: sortBy,
                Page: page,
                PageCount: pageCount,
                Descending: descending,
                IsDeleted: isDeleted));
        
        return Ok(result);
    }
    
    #endregion
}