using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Delivery.Application.Features.DeliveryOrder.Commands.ArriveAtSortingCenter;
using RenStore.Delivery.Application.Features.DeliveryOrder.Commands.DeleteDeliveryOrder;
using RenStore.Delivery.Application.Features.DeliveryOrder.Commands.MarkAsAssembling;
using RenStore.Delivery.Application.Features.DeliveryOrder.Commands.MarkAsAwaitingPickup;
using RenStore.Delivery.Application.Features.DeliveryOrder.Commands.MarkAsDelivered;
using RenStore.Delivery.Application.Features.DeliveryOrder.Commands.Return;
using RenStore.Delivery.Application.Features.DeliveryOrder.Commands.ShipToPickupPoint;
using RenStore.Delivery.Application.Features.DeliveryOrder.Commands.ShipToSortingCenter;
using RenStore.Delivery.Application.Features.DeliveryOrder.Commands.SortAtSortingCenter;
using RenStore.Delivery.Application.Features.DeliveryOrder.Queries.Delivery.FindAllDeliveryOrders;
using RenStore.Delivery.Application.Features.DeliveryOrder.Queries.Delivery.FindDeliveryOrderById;
using RenStore.Delivery.Application.Features.DeliveryOrder.Queries.Tracking.FindTrackingByDeliveryOrderId;
using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Delivery.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Delivery.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/manage/delivery")]
/*[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator},{Roles.Support}")]*/
public sealed class DeliveryManageController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator
        ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("{deliveryOrderId:guid}")]
    [ProducesResponseType(typeof(DeliveryOrderReadModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid deliveryOrderId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindDeliveryOrderByIdQuery(deliveryOrderId),
            cancellationToken);

        return result is not null 
            ? Ok(result) 
            : NotFound();
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DeliveryOrderReadModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] DeliveryOrderSortBy sortBy = DeliveryOrderSortBy.CreatedAt,
        [FromQuery] uint                page = 1,
        [FromQuery] uint                pageSize = 25,
        [FromQuery] bool                descending = true,
        [FromQuery] DeliveryStatus?     status = null,
        CancellationToken               cancellationToken = default)
    {
        var result = await _mediator.Send(
            new FindAllDeliveryOrdersQuery(sortBy, page, pageSize, descending, status),
            cancellationToken);

        return Ok(result);
    }

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

    [HttpPatch("{deliveryOrderId:guid}/assembling")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsAssembling(
        [FromRoute] Guid deliveryOrderId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new MarkAsAssemblingBySellerCommand(deliveryOrderId),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{deliveryOrderId:guid}/ship-to-sorting-center")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ShipToSortingCenter(
        [FromRoute] Guid deliveryOrderId,
        [FromBody]  long sortingCenterId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ShipToSortingCenterCommand(deliveryOrderId, sortingCenterId),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{deliveryOrderId:guid}/arrived-at-sorting-center")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ArrivedAtSortingCenter(
        [FromRoute] Guid deliveryOrderId,
        [FromBody]  long sortingCenterId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ArrivedAtSortingCenterCommand(deliveryOrderId, sortingCenterId),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{deliveryOrderId:guid}/sorted")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Sorted(
        [FromRoute] Guid deliveryOrderId,
        [FromBody]  long sortingCenterId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new SortAtSortingCenterCommand(deliveryOrderId, sortingCenterId),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{deliveryOrderId:guid}/ship-to-pickup-point")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ShipToPickupPoint(
        [FromRoute] Guid deliveryOrderId,
        [FromBody]  long pickupPointId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ShipToPickupPointCommand(deliveryOrderId, pickupPointId),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{deliveryOrderId:guid}/awaiting-pickup")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AwaitingPickup(
        [FromRoute] Guid deliveryOrderId,
        [FromBody]  long pickupPointId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new MarkAsAwaitingPickupCommand(deliveryOrderId, pickupPointId),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{deliveryOrderId:guid}/delivered")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delivered(
        [FromRoute] Guid deliveryOrderId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new MarkAsDeliveredCommand(deliveryOrderId),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{deliveryOrderId:guid}/return")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Return(
        [FromRoute] Guid deliveryOrderId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new ReturnDeliveryOrderCommand(deliveryOrderId),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{deliveryOrderId:guid}")]
    /*[Authorize(Roles = Roles.Admin)]*/
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid deliveryOrderId,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteDeliveryOrderCommand(deliveryOrderId),
            cancellationToken);

        return NoContent();
    }
}