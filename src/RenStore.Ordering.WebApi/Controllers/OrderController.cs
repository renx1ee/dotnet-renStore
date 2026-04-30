using Asp.Versioning;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Order.Application.Enums;
using RenStore.Order.Application.Features.Order.Queries.Orders.FindByOrderById;
using RenStore.Order.Application.Features.Order.Queries.Orders.GetMyOrders;
using RenStore.Order.Application.Saga.Contracts.Commands;
using RenStore.Order.Application.Services;
using RenStore.Ordering.WebApi.Requests;

namespace RenStore.Ordering.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
/*[Authorize(Roles = Roles.Buyer)] */
[Route("api/v{version:apiVersion}/orders")]
public sealed class OrderController(
    IMediator mediator,
    IPublishEndpoint publishEndpoint,
    ICurrentUserService currentUser) : ControllerBase
{
    private readonly IMediator _mediator               = mediator        ?? throw new ArgumentNullException(nameof(mediator));
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
    private readonly ICurrentUserService _currentUser  = currentUser     ?? throw new ArgumentNullException(nameof(currentUser));
    
    #region Commands
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = NewId.NextGuid(); // MassTransit генератор — лучше чем Guid.NewGuid()
        
        if (_currentUser.UserId == Guid.Empty)
            return Forbid();

        await _publishEndpoint.Publish(
            new InitiateOrderPlacement(
                CorrelationId: correlationId,
                CustomerId:    _currentUser.UserId,
                VariantId:     request.VariantId,
                SizeId:        request.SizeId,
                Quantity:      request.Quantity),
            cancellationToken)
            .ConfigureAwait(false);
        
        return Accepted(new { CorrelationId = correlationId });
    }
    
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