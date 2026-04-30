using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Order.Application.Enums;
using RenStore.Order.Application.Features.Order.Queries.Orders.FindByCustomerId;
using RenStore.Order.Application.Features.Order.Queries.Orders.FindById;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Ordering.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/manage/orders")]
/*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator},{Roles.Support}")]*/
public sealed class OrderManageController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    
    #region Commands

    [HttpPatch("{orderId:guid}/cancel")]
    public async Task<IActionResult> Cancel(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        return NoContent();
    }

    [HttpPatch("{orderId:guid}/approve")]
    public async Task<IActionResult> Approve(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        return NoContent();
    }

    [HttpPatch("{orderId:guid}/refund")]
    public async Task<IActionResult> Refund(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        return Accepted();
    }

    #endregion

    #region Queries
    
    [HttpGet]
    [Route("{orderId:guid}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindOrderByIdQuery(orderId),
            cancellationToken);

        return result is not null
            ? Ok(result)
            : NotFound();
    }
    
    [HttpGet]
    [Route("customers/{customerId:guid}")]
    public async Task<IActionResult> GetByCustomerId(
        [FromRoute] Guid customerId,
        [FromQuery] OrderSortBy sortBy = OrderSortBy.CreatedAt,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageSize = 25,
        [FromQuery] bool descending = true,
        [FromQuery] bool onlyActive = false,
        [FromQuery] bool? isDeleted = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new FindOrdersByCustomerIdQuery(
                customerId,
                sortBy,
                page,
                pageSize,
                descending,
                onlyActive,
                isDeleted),
            cancellationToken);

        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        return Ok(); // TODO:
    }

    #endregion
}