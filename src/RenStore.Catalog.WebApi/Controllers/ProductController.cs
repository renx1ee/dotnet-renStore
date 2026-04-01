namespace RenStore.Catalog.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("/api/v{version:apiVersion}")]
public sealed class ProductController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    #region Commands
    
    [HttpPost("manage/categories/{categoryId}/sub-categories/{subCategoryId}/products")]
    /*[Authorize(Roles = Roles.Seller)]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> Create(
        [FromRoute] Guid categoryId,
        [FromRoute] Guid subCategoryId,
        CancellationToken cancellationToken)
    {
        var command = new CreateProductCommand(
            CategoryId: categoryId,
            SubCategoryId: subCategoryId);
        
        var productId = await _mediator.Send(command, cancellationToken);

        return productId == Guid.Empty ? BadRequest() : CreatedAtAction(
            actionName: nameof(FindById),
            routeValues: new { productId = productId },
            value: new { Id = productId });
    }
    
    [HttpPatch("manage/products/{productId:guid}/publish")]
    /*[Authorize(Roles = Roles.Seller)]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> Publish(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new PublishProductCommand(
            ProductId: productId), 
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("manage/products/{productId:guid}/approve")]
    /*[Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> Approve(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new ApproveProductCommand(
            ProductId: productId), 
            cancellationToken);

        return NoContent();
    }
    
    [HttpPatch("manage/products/{productId:guid}/archive")]
    /*[Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> Archive(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new ArchiveProductCommand(
            ProductId: productId), 
            cancellationToken);

        return NoContent();
    }
    
    [HttpPatch("manage/products/{productId:guid}/hide")]
    /*[Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> Hide(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new HideProductCommand(
            ProductId: productId), 
            cancellationToken);

        return NoContent();
    }
    
    [HttpPatch("manage/products/{productId:guid}/reject")]
    /*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator}")]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> Reject(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new RejectProductCommand(
            ProductId: productId), 
            cancellationToken);

        return NoContent();
    }
    
    [HttpPatch("manage/products/{productId:guid}/draft")]
    /*[Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> ToDraft(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DraftProductCommand(
            ProductId: productId), 
            cancellationToken);

        return NoContent();
    }
    
    [HttpDelete("manage/products/{productId:guid}")]
    /*[Authorize(Roles = $"{Roles.Seller},{Roles.Admin},{Roles.Moderator}")]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new SoftDeleteProductCommand(
            ProductId: productId), 
            cancellationToken);

        return NoContent();
    }
    
    #endregion

    #region Queries Catalog
    
    [HttpGet("catalog/products/{productId:guid}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindById(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(
            new FindProductByIdQuery(productId), 
            cancellationToken);
        
        return product is null ? NotFound() : Ok(product);
    }
    
    [HttpGet("catalog/products/{sellerId:guid}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindBySellerId(
        [FromRoute] Guid sellerId,
        CancellationToken cancellationToken,
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
                IsDeleted: false), 
            cancellationToken);
        
        return !products.Any() ? NotFound() : Ok(products);
    }
    
    #endregion
    
    #region Queries Manage
    
    [HttpGet("manage/products/{productId:guid}")]
    [Authorize(Roles = "Seller,Moderator,Admin")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindByIdManage(
        [FromRoute] Guid productId,
        CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(
            new FindProductByIdQuery(
                ProductId: productId), 
            cancellationToken);
        
        return product is null ? NotFound() : Ok(product);
    }
    
    [HttpGet("manage/sellers/{sellerId:guid}/products")]
    [Authorize(Roles = "Seller,Moderator,Admin")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindBySellerIdManage(
        [FromRoute] Guid sellerId,
        CancellationToken cancellationToken,
        [FromQuery] ProductSortBy sortBy = ProductSortBy.Id,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageCount = 25,
        [FromQuery] bool descending = false,
        [FromQuery] bool? isDeleted = null)
    {
        var products = await _mediator.Send(
            new FindProductsBySellerIdQuery(
                SellerId: sellerId,
                SortBy: sortBy,
                Page: page,
                PageCount: pageCount,
                Descending: descending,
                IsDeleted: isDeleted), 
            cancellationToken);
        
        return !products.Any() ? NotFound() : Ok(products);
    }
    
    #endregion
}