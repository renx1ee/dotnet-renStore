using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Catalog.Application.Features.Product.Commands.Approve;
using RenStore.Catalog.Application.Features.Product.Commands.Archive;
using RenStore.Catalog.Application.Features.Product.Commands.Create;
using RenStore.Catalog.Application.Features.Product.Commands.Hide;
using RenStore.Catalog.Application.Features.Product.Commands.PublishProduct;
using RenStore.Catalog.Application.Features.Product.Commands.Reject;
using RenStore.Catalog.Application.Features.Product.Commands.SoftDelete;
using RenStore.Catalog.Application.Features.Product.Commands.ToDraft;
using RenStore.Catalog.Application.Features.Product.Queries.FindById;
using RenStore.Catalog.Application.Features.Product.Queries.FindBySellerId;
using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Catalog.WebApi.Requests;
using RenStore.Catalog.WebApi.Requests.Product;

namespace RenStore.Catalog.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("/api/v{version:apiVersion}/catalog/products")]
public sealed class ProductController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    
    [HttpPost]
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
    
    [HttpPatch("{id:guid}/publish")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Publish(Guid id)
    {
        await _mediator.Send(new PublishProductCommand(id));

        return NoContent();
    }

    [HttpPatch("{id:guid}/approve")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _mediator.Send(new ApproveProductCommand(id));

        return NoContent();
    }
    
    [HttpPatch("{id:guid}/archive")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Archive(Guid id)
    {
        await _mediator.Send(new ArchiveProductCommand(id));

        return NoContent();
    }
    
    [HttpPatch("{id:guid}/hide")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Hide(Guid id)
    {
        await _mediator.Send(new HideProductCommand(id));

        return NoContent();
    }
    
    [HttpPatch("{id:guid}/reject")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Reject(Guid id)
    {
        await _mediator.Send(new RejectProductCommand(id));

        return NoContent();
    }
    
    [HttpPatch("{id:guid}/draft")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> ToDraft(Guid id)
    {
        await _mediator.Send(new DraftProductCommand(id));

        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    /*[Authorize(Roles = "Seller,Moderator,Admin")]*/
    [MapToApiVersion(1)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new SoftDeleteProductCommand(id));

        return NoContent();
    }
    
    [HttpGet("{productId:guid}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindById(Guid productId)
    {
        var product = await _mediator.Send(
            new FindProductByIdQuery(productId));
        
        return product is null ? NotFound() : Ok(product);
    }
    
    [HttpGet("{sellerId:long}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> FindBySellerId(
        long sellerId,
        [FromQuery] ProductSortBy sortBy = ProductSortBy.Id,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageCount = 25,
        [FromQuery] bool descending = false,
        [FromQuery] bool? isDeleted = null)
    {
        var products = await _mediator.Send(
            new FindProductBySellerIdQuery(
                SellerId: sellerId,
                SortBy: sortBy,
                Page: page,
                PageCount: pageCount,
                Descending: descending,
                IsDeleted: isDeleted));
        
        return !products.Any() ? NotFound() : Ok(products);
    }

    /*[HttpGet]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25)
    {
        return Ok();
    }*/
}