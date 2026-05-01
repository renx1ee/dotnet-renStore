using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenStore.Payment.Application.Features.Payment.Commands.Cancel;
using RenStore.Payment.Application.Features.Payment.Commands.Capture;
using RenStore.Payment.Application.Features.Payment.Commands.InitiateRefund;
using RenStore.Payment.Application.Features.Payment.Queries.Attempts;
using RenStore.Payment.Application.Features.Payment.Queries.Payment.FindAll;
using RenStore.Payment.Application.Features.Payment.Queries.Payment.FindByCustomerId;
using RenStore.Payment.Application.Features.Payment.Queries.Payment.FindById;
using RenStore.Payment.Application.Features.Payment.Queries.Refund.FindRefundsByPaymentId;
using RenStore.Payment.Domain.Enums;
using RenStore.Payment.Domain.ReadModels;
using RenStore.Payment.WebApi.Requests;
using RenStore.SharedKernal.Domain.Constants;

namespace RenStore.Payment.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/manage/payments")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Moderator},{Roles.Support}")]
public sealed class PaymentManageController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    #region Commands

    /// <summary>
    /// Ручной capture — на случай если webhook не пришёл.
    /// </summary>
    [HttpPatch("{paymentId:guid}/capture")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Capture(
        [FromRoute] Guid   paymentId,
        [FromBody]  string externalPaymentId,
        CancellationToken  cancellationToken)
    {
        await _mediator.Send(
            new CapturePaymentCommand(paymentId, externalPaymentId),
            cancellationToken);

        return NoContent();
    }
    
    /// <summary>
    /// Ручная отмена — для поддержки.
    /// </summary>
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

    /// <summary>
    /// Возврат — инициируется поддержкой.
    /// </summary>
    [HttpPost("{paymentId:guid}/refunds")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Refund(
        [FromRoute] Guid                 paymentId,
        [FromBody]  InitiateRefundRequest request,
        CancellationToken                cancellationToken)
    {
        var refundId = await _mediator.Send(
            new InitiateRefundCommand(paymentId, request.Amount, request.Reason),
            cancellationToken);

        return Created(string.Empty, refundId);
    }

    #endregion

    #region Queries

    [HttpGet("{paymentId:guid}")]
    [ProducesResponseType(typeof(PaymentReadModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid paymentId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindPaymentByIdQuery(paymentId),
            cancellationToken);

        return result is not null 
            ? Ok(result) 
            : NotFound();
    }
    
    [HttpGet("customers/{customerId:guid}")]
    [ProducesResponseType(typeof(IReadOnlyList<PaymentReadModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByCustomerId(
        [FromRoute] Guid           customerId,
        [FromQuery] PaymentSortBy  sortBy = PaymentSortBy.CreatedAt,
        [FromQuery] uint           page = 1,
        [FromQuery] uint           pageSize = 25,
        [FromQuery] bool           descending = true,
        [FromQuery] PaymentStatus? status = null,
        CancellationToken          cancellationToken = default)
    {
        var result = await _mediator.Send(
            new FindPaymentsByCustomerIdQuery(
                customerId, sortBy, page, pageSize, descending, status),
            cancellationToken);

        return Ok(result);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PaymentReadModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaymentSortBy  sortBy = PaymentSortBy.CreatedAt,
        [FromQuery] uint           page = 1,
        [FromQuery] uint           pageSize = 25,
        [FromQuery] bool           descending = true,
        [FromQuery] PaymentStatus? status = null,
        CancellationToken          cancellationToken = default)
    {
        var result = await _mediator.Send(
            new FindAllPaymentsQuery(sortBy, page, pageSize, descending, status),
            cancellationToken);

        return Ok(result);
    }
    
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

    [HttpGet("{paymentId:guid}/attempts")]
    [ProducesResponseType(typeof(IReadOnlyList<PaymentAttemptReadModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAttempts(
        [FromRoute] Guid paymentId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new FindAttemptsByPaymentIdQuery(paymentId),
            cancellationToken);

        return Ok(result);
    }

    #endregion
}