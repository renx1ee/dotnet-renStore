using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RenStore.Payment.Application.Abstractions.Queries;
using RenStore.Payment.Application.Features.Payment.Commands.Capture;
using RenStore.Payment.Application.Features.Payment.Commands.Fail;
using RenStore.Payment.Application.Features.Payment.Commands.RefundFail;
using RenStore.Payment.Application.Features.Payment.Commands.RefundSuccess;
using RenStore.Payment.WebApi.Requests;

namespace RenStore.Payment.WebApi.Controllers;

[ApiController]
[ApiVersion(1, Deprecated = false)]
[Route("api/v{version:apiVersion}/webhooks/yookassa")]
public sealed class YooKassaWebhookController(
    IMediator     mediator,
    IPaymentQuery paymentQuery) : ControllerBase
{
    private readonly IMediator     _mediator     = mediator;
    private readonly IPaymentQuery _paymentQuery = paymentQuery;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Handle(
        [FromBody] YooKassaWebhookPayload payload,
        CancellationToken cancellationToken)
    {
        var obj = payload.Object;
        
        var payment = await _paymentQuery.FindByExternalPaymentIdAsync(
            obj.Id,
            cancellationToken);
        
        if (payment is null)
            return Ok();
        
        switch (payload.Event)
        {
            // Деньги заморожены — делаем capture
            case "payment.waiting_for_capture":
                await _mediator.Send(
                    new CapturePaymentCommand(
                        PaymentId:         payment.Id,
                        ExternalPaymentId: obj.Id),
                    cancellationToken);
                break;

            // Платёж отменён/отклонён
            case "payment.canceled":
                await _mediator.Send(
                    new FailPaymentCommand(
                        PaymentId:         payment.Id,
                        AttemptId:         payment.LastAttemptId!.Value,
                        Reason:            obj.CancellationDetails?.Reason ?? "Cancelled by provider",
                        ProviderErrorCode: obj.CancellationDetails?.Party),
                    cancellationToken);
                break;

            // Возврат завершён успешно
            case "refund.succeeded":
                if (obj.RefundId.HasValue)
                    await _mediator.Send(
                        new SucceedRefundCommand(
                            PaymentId:        payment.Id,
                            RefundId:         obj.RefundId.Value,
                            ExternalRefundId: obj.Id),
                        cancellationToken);
                break;

            // Возврат отклонён
            case "refund.canceled":
                if (obj.RefundId.HasValue)
                    await _mediator.Send(
                        new FailRefundCommand(
                            PaymentId: payment.Id,
                            RefundId:  obj.RefundId.Value,
                            Reason:    "Refund cancelled by provider"),
                        cancellationToken);
                break;
        }

        return Ok();
    }
}

public sealed class YooKassaWebhookPayload
{
    public string             Event  { get; init; } = null!;
    public YooKassaObject     Object { get; init; } = null!;
}

public sealed class YooKassaObject
{
    public string                      Id                  { get; init; } = null!;
    public string                      Status              { get; init; } = null!;
    public Guid?                       RefundId            { get; init; }
    public YooKassaCancellationDetails? CancellationDetails { get; init; }
}

public sealed class YooKassaCancellationDetails
{
    public string? Party  { get; init; }
    public string? Reason { get; init; }
}