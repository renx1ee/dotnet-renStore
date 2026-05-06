using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Delivery.Application.Features.DeliveryOrder.Queries.Delivery.FindDeliveryOrderByOrderId;
using RenStore.Delivery.Application.Features.DeliveryOrder.Queries.Tracking.FindTrackingByDeliveryOrderId;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Delivery.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/delivery")]
/*[Authorize(Roles = Roles.Buyer)]*/
public sealed class DeliveryOrderController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator
                                           ?? throw new ArgumentNullException(nameof(mediator));

    /// <summary>Получить доставку по ID заказа.</summary>
    [HttpGet("orders/{orderId:guid}")]
    [ProducesResponseType(typeof(DeliveryOrderReadModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByOrderId(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindDeliveryOrderByOrderIdQuery(orderId),
            cancellationToken);

        return result is not null ? Ok(result) : NotFound();
    }

    /// <summary>Получить историю трекинга.</summary>
    [HttpGet("{deliveryOrderId:guid}/tracking")]
    [ProducesResponseType(typeof(IReadOnlyList<DeliveryTrackingReadModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTracking(
        [FromRoute] Guid deliveryOrderId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindTrackingByDeliveryOrderIdQuery(deliveryOrderId),
            cancellationToken);

        return Ok(result);
    }
}