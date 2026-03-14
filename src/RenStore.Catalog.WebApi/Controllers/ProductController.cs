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
        
        var result = await _mediator.Send(command);

        return result == Guid.Empty ? BadRequest() : Created();
    }
    
    [HttpDelete("{id:guid}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new SoftDeleteProductCommand(id));

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
    
    [HttpPatch("{id:guid}/publish")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> Publish(Guid id)
    {
        await _mediator.Send(new PublishProductCommand(id));

        return NoContent();
    }
    
    
    /*[HttpGet]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 25)
    {
        return Ok();
    }
    
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetById(Guid id)
    {
        /*return product is null ? NotFound() : Ok();#1#
        return Ok();
    }
    
    [HttpGet("{article:int}")]
    [AllowAnonymous]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetByArticle(int article)
    {
        return Ok();
    }*/
}