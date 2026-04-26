using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Order.Application.Enums;
using RenStore.Order.Application.Features.Order.Queries.OrderItems.FindItemsByOrderId;
using RenStore.Order.Application.Features.Order.Queries.OrderItems.FindOrderItemById;
using RenStore.Order.Application.Features.Order.Queries.OrderItems.FindOrderItemsByVariantId;
using RenStore.Order.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Ordering.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/manage/order-items")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator},{Roles.Support}")]
public sealed class OrderItemManageController(IMediator mediator) : ControllerBase
{
    [HttpGet("{orderItemId:guid}")]
    [ProducesResponseType(typeof(OrderItemReadModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> FindByIdAsync(
        [FromRoute] Guid orderItemId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new FindOrderItemByIdQuery(orderItemId),
            cancellationToken);

        return result is null
            ? NotFound()
            : Ok(result);
    }
    
    [HttpGet("orders/{orderId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<OrderItemReadModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> FindByOrderIdAsync(
        [FromRoute] Guid orderId,
        [FromQuery] OrderItemSortBy sortBy = OrderItemSortBy.CreatedAt,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageSize = 25,
        [FromQuery] bool descending = true,
        [FromQuery] bool onlyActive = false,
        [FromQuery] bool? isDeleted = null,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new FindOrderItemsByOrderIdQuery(
                orderId,
                sortBy,
                page,
                pageSize,
                descending,
                onlyActive,
                isDeleted),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("variants/{variantId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<OrderItemReadModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> FindByVariantIdAsync(
        [FromRoute] Guid variantId,
        [FromQuery] OrderItemSortBy sortBy = OrderItemSortBy.CreatedAt,
        [FromQuery] uint page = 1,
        [FromQuery] uint pageSize = 25,
        [FromQuery] bool descending = true,
        [FromQuery] bool onlyActive = false,
        [FromQuery] bool? isDeleted = null,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(
            new FindOrderItemsByVariantIdQuery(
                variantId,
                sortBy,
                page,
                pageSize,
                descending,
                onlyActive,
                isDeleted),
            cancellationToken);

        return Ok(result);
    }
}
