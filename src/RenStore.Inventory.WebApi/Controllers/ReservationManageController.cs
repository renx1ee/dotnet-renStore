using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Inventory.Application.Features.Reservation.Commands.Cancel;
using RenStore.Inventory.Application.Features.Reservation.Commands.SoftDelete;
using RenStore.Inventory.Application.Features.Reservation.Queries.FindById;
using RenStore.Inventory.Application.Features.Reservation.Queries.FindByOrderId;
using RenStore.Inventory.Application.Features.Reservation.Queries.FindByVariantId;
using RenStore.Inventory.Application.Features.Reservation.Queries.FindByVariantIdAndSizeId;
using RenStore.Inventory.Application.Features.Reservation.Queries.FindExpired;
using RenStore.Inventory.WebApi.Requests.Reservation;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Inventory.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator},{Roles.Support}")]
[Route("api/v{version:apiVersion}/manage/reservations")] // есть еще один контроллер для публичного доступа (без параметров include_deleted etc.) 
public class ReservationManageController(IMediator mediator) : ControllerBase
{
    #region Commands

    [HttpPatch("{reservationId:guid}/cancel")]
    [ApiVersion(1)]
    public async Task<IActionResult> Cancel(
        [FromRoute] Guid reservationId,
        [FromQuery] CancelReservationRequest request,
        CancellationToken cancellationToken)
    {
        await mediator.Send(
            new CancelReservationCommand(
                ReservationId: reservationId,
                Reason: request.Reason), 
            cancellationToken);

        return NoContent();
    }
    
    [HttpDelete("{reservationId:guid}/delete")]
    [ApiVersion(1)]
    public async Task<IActionResult> SoftDelete(
        [FromRoute] Guid reservationId,
        CancellationToken cancellationToken)
    {
        await mediator.Send(
            new SoftDeleteReservationCommand(reservationId), 
            cancellationToken);
        
        return NoContent();
    }

    #endregion
    
    #region Queries

    [HttpGet("{reservationId:guid}")]
    public async Task<IActionResult> FindById(
        [FromRoute] Guid reservationId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new FindReservationByIdQuery(reservationId), 
            cancellationToken);
        
        return result is not null 
            ? Ok(result) 
            : NotFound();
    }
    
    [HttpGet("variants/{variantId:guid}")]
    public async Task<IActionResult> FindByVariantId(
        [FromQuery] FindReservationByVariantIdRequest request,
        [FromRoute] Guid variantId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new FindReservationByVariantIdQuery(
                VariantId: variantId,
                SortBy: request.SortBy,
                Page: request.Page,
                PageSize: request.PageSize,
                Descending: request.Descending,
                IncludeExpired: request.IncludeExpired,
                IsDeleted: request.IsDeleted),
            cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("variants/{variantId:guid}/sizes/{sizeId:guid}")]
    public async Task<IActionResult> FindByVariantIdAndSizeId(
        [FromRoute] Guid variantId,
        [FromRoute] Guid sizeId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new FindReservationByVariantAndSizeQuery(
                VariantId: variantId,
                SizeId: sizeId),
            cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("orders/{orderId:guid}")]
    public async Task<IActionResult> FindByOrderId(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new FindReservationByOrderIdQuery(
                OrderId: orderId),
            cancellationToken);
        
        return Ok(result);
    }
    
    [HttpGet("expired")]
    public async Task<IActionResult> FindByExpired(
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new FindExpiredReservationsQuery(), 
            cancellationToken);
        
        return Ok(result);
    }

    #endregion
}