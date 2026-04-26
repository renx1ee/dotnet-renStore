using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Order.Application.Enums;
using RenStore.Order.Application.Features.Order.Queries.Orders.FindByCustomerId;
using RenStore.Order.Application.Features.Order.Queries.Orders.FindByOrderById;
using RenStore.Order.Application.Features.Order.Queries.Orders.GetMyOrders;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Ordering.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Authorize(Roles = Roles.Buyer)] 
[Route("api/v{version:apiVersion}/orders")]
public sealed class OrderController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    
    #region Commands
    
    [HttpPatch]
    [Route("{orderId:guid}/cancel")]
    public async Task<IActionResult> Cancel(
        CancellationToken cancellationToken)
    {
        return NoContent();
    }

    #endregion
    
    #region Queries
    
    // статус своего заказа
    [HttpGet]
    [Route("{orderId:guid}")] 
    public async Task<IActionResult> GetById(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindMyOrderByIdQuery(orderId), 
            cancellationToken);
        
        return result is not null 
            ? Ok(result) 
            : NotFound();
    }
    
    // список своих заказов
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] OrderSortBy sortBy = OrderSortBy.CreatedAt,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageSize = 25,
        [FromQuery] bool descending = true,
        [FromQuery] bool onlyActive = false,
        [FromQuery] bool? isDeleted = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new FindMyOrdersByCustomerIdQuery(
                sortBy,
                page,
                pageSize,
                descending,
                onlyActive,
                isDeleted),
            cancellationToken);

        return Ok(result);
    }

    #endregion
}