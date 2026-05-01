using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Domain.Constants;
using RenStore.Payment.Application.Features.Payment.Commands.Cancel;
using RenStore.Payment.Application.Features.Payment.Commands.Create;
using RenStore.Payment.Application.Features.Payment.Commands.InitiatePayment;
using RenStore.Payment.Application.Features.Payment.Commands.InitiateRefund;
using RenStore.Payment.Application.Features.Payment.Queries.Payment.FindByOrderId;
using RenStore.Payment.Application.Features.Payment.Queries.Payment.FindMyPayments;
using RenStore.Payment.Application.Features.Payment.Queries.Refund.FindRefundsByPaymentId;
using RenStore.Payment.Domain.Enums;
using RenStore.Payment.Domain.ReadModels;
using RenStore.Payment.WebApi.Requests;

namespace RenStore.Payment.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/payments")]
[Authorize(Roles = Roles.User)]
public sealed class PaymentController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    #region Commands
    
    /// <summary>
    /// Инициирует оплату — вызывает YooKassa и редиректит покупателя на страницу оплаты.
    /// </summary>
    [HttpPost("{paymentId:guid}/initiate")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Initiate(
        [FromRoute] Guid              paymentId,
        [FromBody]  InitiatePaymentRequest request,
        CancellationToken                  cancellationToken)
    {
        var result = await _mediator.Send(
            new InitiatePaymentCommand(paymentId, request.Description),
            cancellationToken);

        return Redirect(result.ConfirmationUrl);
    }
    
    /// <summary>Cancel a pending payment.</summary>
    [HttpPatch("{paymentId:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(
        [FromRoute] Guid   paymentId,
        [FromBody]  string reason,
        CancellationToken  cancellationToken)
    {
        await _mediator.Send(
            new CancelPaymentCommand(paymentId, reason),
            cancellationToken);

        return NoContent();
    }
    
    /// <summary>Initiate a refund for a captured payment.</summary>
    [HttpPost("{paymentId:guid}/refunds")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Refund(
        [FromRoute] Guid                paymentId,
        [FromBody]  InitiateRefundRequest request,
        CancellationToken               cancellationToken)
    {
        var refundId = await _mediator.Send(
            new InitiateRefundCommand(paymentId, request.Amount, request.Reason),
            cancellationToken);

        return Created(string.Empty, refundId);
    }
    
    #endregion

    #region Queries

    /// <summary>Get payment by order id.</summary>
    [HttpGet("orders/{orderId:guid}")]
    [ProducesResponseType(typeof(PaymentReadModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByOrderId(
        [FromRoute] Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindPaymentByOrderIdQuery(orderId),
            cancellationToken);

        return result is not null ? Ok(result) : NotFound();
    }
    
    /// <summary>Get current user's payments.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PaymentReadModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMy(
        [FromQuery] PaymentSortBy  sortBy = PaymentSortBy.CreatedAt,
        [FromQuery] uint           page = 1,
        [FromQuery] uint           pageSize = 25,
        [FromQuery] bool           descending = true,
        [FromQuery] PaymentStatus? status = null,
        CancellationToken          cancellationToken = default)
    {
        var result = await _mediator.Send(
            new FindMyPaymentsQuery(sortBy, page, pageSize, descending, status),
            cancellationToken);

        return Ok(result);
    }
    
    /// <summary>
    /// Получить возвраты по paymentId.
    /// </summary>
    [HttpGet("{paymentId:guid}/refunds")]
    [ProducesResponseType(typeof(IReadOnlyList<RefundReadModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRefunds(
        [FromRoute] Guid paymentId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindRefundsByPaymentIdQuery(paymentId),
            cancellationToken);

        return Ok(result);
    }

    #endregion
}