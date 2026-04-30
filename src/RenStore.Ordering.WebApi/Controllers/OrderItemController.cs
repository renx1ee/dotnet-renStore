using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Order.Application.Enums;
using RenStore.Order.Application.Features.Order.Queries.OrderItems.FindMyItemsByOrderId;
using RenStore.Order.Application.Features.Order.Queries.Orders.FindByOrderById;
using RenStore.Order.Application.Features.Order.Queries.Orders.GetMyOrders;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Ordering.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/order-items")]
/*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator},{Roles.Support}")]*/
public class OrderItemController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    
    [HttpGet]
    [Route("{itemId:guid}")] 
    public async Task<IActionResult> GetById(
        [FromRoute] Guid itemId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindMyOrderItemsByOrderIdQuery(itemId), 
            cancellationToken);

        return Ok(result);
    }
}