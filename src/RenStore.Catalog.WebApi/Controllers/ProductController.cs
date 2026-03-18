namespace RenStore.Catalog.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("/api/v{version:apiVersion}")]
public sealed class ProductController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    #region Commands
    
    [HttpPost("manage/products")]
    [Authorize(Roles = "Seller")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Create(
        [FromQuery] Guid subCategoryId)
    {
        var command = new CreateProductCommand(
            UserId: User.GetUserId(),
            SubCategoryId: subCategoryId);
        
        var productId = await _mediator.Send(command);

        return productId == Guid.Empty ? BadRequest() : CreatedAtAction(
            actionName: nameof(FindById),
            routeValues: new { result = productId, version = "1" },
            value: new { Id = productId });
    }
    
    [HttpPatch("manage/products/{productId:guid}/publish")]
    [Authorize("Seller")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Publish(Guid productId)
    {
        await _mediator.Send(new PublishProductCommand(
            ProductId: productId,
            Role: User.GetRole(),
            UserId: User.GetUserId()));

        return NoContent();
    }

    [HttpPatch("manage/products/{productId:guid}/approve")]
    [Authorize(Roles = "Admin,Moderator")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Approve(Guid productId)
    {
        await _mediator.Send(new ApproveProductCommand(
            ProductId: productId,
            Role: User.GetRole(),
            UserId: User.GetUserId()));

        return NoContent();
    }
    
    [HttpPatch("manage/products/{productId:guid}/archive")]
    [Authorize(Roles = "Seller,Admin,Moderator")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Archive(Guid productId)
    {
        await _mediator.Send(new ArchiveProductCommand(
            ProductId: productId,
            Role: User.GetRole(),
            UserId: User.GetUserId()));

        return NoContent();
    }
    
    [HttpPatch("manage/products/{productId:guid}/hide")]
    [Authorize(Roles = "Seller,Admin,Moderator")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Hide(Guid productId)
    {
        await _mediator.Send(new HideProductCommand(
            ProductId: productId,
            Role: User.GetRole(),
            UserId: User.GetUserId()));

        return NoContent();
    }
    
    [HttpPatch("manage/products/{productId:guid}/reject")]
    [Authorize(Roles = "Admin,Moderator")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Reject(Guid productId)
    {
        await _mediator.Send(new RejectProductCommand(
            ProductId: productId,
            Role: User.GetRole(),
            UserId: User.GetUserId()));

        return NoContent();
    }
    
    [HttpPatch("manage/products/{productId:guid}/draft")]
    [Authorize(Roles = "Seller,Admin,Moderator")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> ToDraft(Guid productId)
    {
        await _mediator.Send(new DraftProductCommand(
            ProductId: productId,
            Role: User.GetRole(),
            UserId: User.GetUserId()));

        return NoContent();
    }
    
    [HttpDelete("manage/products/{productId:guid}")]
    [Authorize(Roles = "Seller,Moderator,Admin")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Delete(Guid productId)
    {
        await _mediator.Send(new SoftDeleteProductCommand(
            ProductId: productId,
            Role: User.GetRole(),
            UserId: User.GetUserId()));

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
    
    [HttpGet("manage/sellers/{sellerId:guid}/products")]
    [Authorize(Roles = "Seller,Moderator,Admin")]
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