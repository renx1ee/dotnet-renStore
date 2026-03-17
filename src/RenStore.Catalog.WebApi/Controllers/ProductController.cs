namespace RenStore.Catalog.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("/api/v{version:apiVersion}")]
public sealed class ProductController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    #region Commands
    
    [HttpPost("manage/products")]
    [Authorize("Seller")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductRequest request)
    {
        var command = new CreateProductCommand(
            SellerId: request.SellerId,
            SubCategoryId: request.SubCategoryId);
        
        var productId = await _mediator.Send(command);

        return productId == Guid.Empty ? BadRequest() : CreatedAtAction(
            actionName: nameof(FindById),
            routeValues: new { result = productId, version = "1" },
            value: new { Id = productId });
    }
    
    [HttpPatch("manage/products/{id:guid}/publish")]
    [Authorize("Seller")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Publish(Guid id)
    {
        await _mediator.Send(new PublishProductCommand(id));

        return NoContent();
    }

    [HttpPatch("manage/products/{id:guid}/approve")]
    [Authorize("Admin,Moderator")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _mediator.Send(new ApproveProductCommand(id));

        return NoContent();
    }
    
    [HttpPatch("manage/products/{id:guid}/archive")]
    [Authorize("Seller")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Archive(Guid id)
    {
        await _mediator.Send(new ArchiveProductCommand(id));

        return NoContent();
    }
    
    [HttpPatch("manage/products/{id:guid}/hide")]
    [Authorize("Seller,Admin,Moderator")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Hide(Guid id)
    {
        await _mediator.Send(new HideProductCommand(id));

        return NoContent();
    }
    
    [HttpPatch("manage/products/{id:guid}/reject")]
    [Authorize("Admin,Moderator")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Reject(Guid id)
    {
        await _mediator.Send(new RejectProductCommand(id));

        return NoContent();
    }
    
    [HttpPatch("manage/products/{id:guid}/draft")]
    [Authorize("Seller,Admin,Moderator")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> ToDraft(Guid id)
    {
        await _mediator.Send(new DraftProductCommand(id));

        return NoContent();
    }
    
    [HttpDelete("manage/products/{id:guid}")]
    [Authorize(Roles = "Seller,Moderator,Admin")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new SoftDeleteProductCommand(id));

        return NoContent();
    }
    
    #endregion

    #region Queries Catalog
    
    [HttpGet("catalog/products/{productId:guid}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindById(Guid productId)
    {
        var product = await _mediator.Send(
            new FindProductByIdQuery(productId));
        
        return product is null ? NotFound() : Ok(product);
    }
    
    [HttpGet("catalog/products/{sellerId:guid}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindBySellerId(
        Guid sellerId,
        [FromQuery] ProductSortBy sortBy = ProductSortBy.Id,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageCount = 25,
        [FromQuery] bool descending = false)
    {
        var products = await _mediator.Send(
            new FindProductsBySellerIdQuery(
                SellerId: sellerId,
                SortBy: sortBy,
                Page: page,
                PageCount: pageCount,
                Descending: descending,
                IsDeleted: false));
        
        return !products.Any() ? NotFound() : Ok(products);
    }
    
    #endregion
    
    #region Queries Manage
    
    [HttpGet("manage/products/{productId:guid}")]
    [Authorize(Roles = "Seller,Moderator,Admin")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByIdManage(Guid productId)
    {
        var product = await _mediator.Send(
            new FindProductByIdQuery(
                ProductId: productId,
                Role: User.GetRole(),
                UserId: User.GetUserId()));
        
        return product is null ? NotFound() : Ok(product);
    }
    
    [HttpGet("manage/products/{sellerId:guid}")]
    [Authorize(Roles = "Seller,Moderator,Admin")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindBySellerIdManage(
        Guid sellerId,
        [FromQuery] ProductSortBy sortBy = ProductSortBy.Id,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageCount = 25,
        [FromQuery] bool descending = false,
        [FromQuery] bool? isDeleted = null)
    {
        var products = await _mediator.Send(
            new FindProductsBySellerIdQuery(
                SellerId: sellerId,
                Role: User.GetRole(),
                UserId: User.GetUserId(),
                SortBy: sortBy,
                Page: page,
                PageCount: pageCount,
                Descending: descending,
                IsDeleted: isDeleted));
        
        return !products.Any() ? NotFound() : Ok(products);
    }
    
    #endregion
}