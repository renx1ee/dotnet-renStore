using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RenStore.Payment.Application.Abstractions.Services;
using RenStore.Payment.Application.Contracts.Requests;
using RenStore.Payment.Application.Options;
using RenStore.Payment.Domain.Interfaces;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Payment.Application.Features.Payment.Commands.InitiatePayment;

internal sealed class InitiatePaymentCommandHandler(
    IPaymentRepository paymentRepository,
    IPaymentProviderService providerService,
    ILogger<InitiatePaymentCommandHandler> logger,
    IOptions<YooKassaOptions> options)
    : IRequestHandler<InitiatePaymentCommand, InitiatePaymentResult>
{
    private readonly YooKassaOptions _options = options.Value;
    
    public async Task<InitiatePaymentResult> Handle(
        InitiatePaymentCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Handling {Command}. PaymentId={PaymentId}",
            nameof(InitiatePaymentCommand),
            request.PaymentId);

        var payment = await paymentRepository.GetAsync(request.PaymentId, cancellationToken);

        if (payment is null)
        {
            throw new NotFoundException(
                name: typeof(Domain.Aggregates.Payment.Payment), request.PaymentId);
        }
        
        var attemptId = payment.CreateAttempt(DateTimeOffset.UtcNow);

        var returnUrl = $"{_options.ReturnUrl}?paymentId={payment.Id}";
        
        var providerResult = await providerService.CreateAsync(
            new CreateProviderPaymentRequest(
                PaymentId:   payment.Id,
                Amount:      payment.Amount,
                Currency:    payment.Currency.ToString(),
                Description: request.Description,
                ReturnUrl:   returnUrl),
            cancellationToken);
        
        if (!providerResult.IsSucceeded)
        {
            payment.Fail(
                now:               DateTimeOffset.UtcNow,
                attemptId:         attemptId,
                reason:            "Provider rejected payment creation.",
                providerErrorCode: null);

            await paymentRepository.SaveAsync(payment, cancellationToken);

            throw new InvalidOperationException(
                $"Payment provider rejected payment. PaymentId={payment.Id}");
        }

        payment.Authorize(
            now:               DateTimeOffset.UtcNow,
            attemptId:         attemptId,
            externalPaymentId: providerResult.ExternalPaymentId,
            externalAuthCode:  null);

        await paymentRepository.SaveAsync(payment, cancellationToken);

        logger.LogInformation(
            "Payment initiated. PaymentId={PaymentId} AttemptId={AttemptId} ExternalId={ExternalId}",
            payment.Id,
            attemptId,
            providerResult.ExternalPaymentId);
        
        return new InitiatePaymentResult(
            PaymentId:       payment.Id,
            AttemptId:       attemptId,
            ConfirmationUrl: providerResult.ConfirmationUrl);
    }
}