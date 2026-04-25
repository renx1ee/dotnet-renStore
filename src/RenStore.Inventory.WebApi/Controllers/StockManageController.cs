using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Inventory.Application.Features.Stock.Commands.AddToStock;
using RenStore.Inventory.Application.Features.Stock.Commands.Set;
using RenStore.Inventory.Application.Features.Stock.Commands.WriteOff;
using RenStore.Inventory.Application.Features.Stock.Queries.FindById;
using RenStore.Inventory.Application.Features.Stock.Queries.FindByVariantId;
using RenStore.Inventory.Application.Features.Stock.Queries.FindStockByVariantIdAndSizeId;
using RenStore.Inventory.WebApi.Requests;
using RenStore.Inventory.WebApi.Requests.Stock;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Inventory.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/manage/stocks")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator},{Roles.Support}")]
public sealed class StockManageController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    
    #region Commands
    
    [HttpPatch("{stockId}/add")]
    /*[Authorize(Roles = "Seller")]*/
    [ApiVersion(1)]
    public async Task<IActionResult> AddToStock(
        [FromRoute] Guid stockId,
        [FromBody] AddToStockRequest request,
        CancellationToken cancellationToken) 
    {
        await _mediator.Send(
            new AddToStockCommand(
                StockId: stockId,
                Count: request.Count), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPatch("{stockId}/write-off")]
    /*[Authorize(Roles = "Seller")]*/
    [ApiVersion(1)]
    public async Task<IActionResult> WriteOff(
        [FromRoute] Guid stockId,
        [FromBody] StockWriteOffRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new StockWriteOffCommand(
                StockId: stockId,
                Count: request.Count,
                Reason: request.Reason), 
            cancellationToken);
        
        return NoContent();
    }
    
    [HttpPatch("{stockId}/set")]
    /*[Authorize(Roles = "Seller")]*/
    [ApiVersion(1)]
    public async Task<IActionResult> Set(
        [FromRoute] Guid stockId,
        [FromBody] SetStockRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new SetStockCommand(
                StockId: stockId,
                Count: request.Count), 
            cancellationToken);
        
        return NoContent();
    }
    
    #endregion

    #region Queries Manage

    [HttpGet("{stockId}")]
    [ApiVersion(1)]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Moderator},{Roles.Support}")]
    public async Task<IActionResult> FindById(
        [FromQuery] Guid stockId,
        [FromBody] FindStockByIdManageRequest request)
    {
        var result = await _mediator.Send(
            new FindStockByIdAsyncQuery(
                StockId: stockId,
                IsDeleted: request.IsDeleted));
        
        return result is not null 
            ? Ok() 
            : NotFound();
    }
    
    [HttpGet("variants/{variantId}")]
    [ApiVersion(1)]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Moderator},{Roles.Support}")]
    public async Task<IActionResult> FindByVariantId(
        [FromQuery] Guid variantId,
        [FromBody] FindStockByVariantIdManageRequest request)
    {
        var result = await _mediator.Send(
            new FindStockByVariantIdQuery(
                VariantId: variantId,
                IsDeleted: request.IsDeleted));

        return Ok(result);
    }
    
    [HttpGet("variants/{variantId}/sizes/{sizeId}")] 
    [ApiVersion(1)]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Moderator},{Roles.Support}")]
    public async Task<IActionResult> FindByVariantIdAndSizeId(
        [FromQuery] Guid variantId,
        [FromQuery] Guid sizeId,
        [FromBody] FindStockByVariantIdAndSizeIdManageRequest request)
    {
        var result = await _mediator.Send(
            new FindStockByVariantIdAndSizeIdQuery(
                VariantId: variantId,
                SizeId: sizeId,
                IsDeleted: request.IsDeleted));
        
        return Ok(result);
    }

    #endregion
}