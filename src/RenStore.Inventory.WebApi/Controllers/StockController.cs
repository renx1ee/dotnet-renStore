using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RenStore.Inventory.Application.Features.Stock.Commands.AddToStock;
using RenStore.Inventory.Application.Features.Stock.Commands.Set;
using RenStore.Inventory.Application.Features.Stock.Commands.WriteOff;
using RenStore.Inventory.WebApi.Requests;

namespace RenStore.Inventory.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/manage/stocks")]
public sealed class StockController(IMediator mediator) : ControllerBase
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
}