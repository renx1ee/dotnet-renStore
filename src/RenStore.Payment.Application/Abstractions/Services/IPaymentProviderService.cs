using RenStore.Payment.Application.Contracts.Requests;

namespace RenStore.Payment.Application.Abstractions.Services;

public interface IPaymentProviderService
{
    /// <summary>
    /// Creates a payment with the provider and returns a URL to redirect the buyer to.
    /// </summary>
    Task<CreateProviderPaymentResult> CreateAsync(
        CreateProviderPaymentRequest request,
        CancellationToken            cancellationToken);

    /// <summary>
    /// Confirms the write-off (capture) after authorization.
    /// </summary>
    Task<bool> CaptureAsync(
        string            externalPaymentId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Cancels payment to the provider.
    /// </summary>
    Task<bool> CancelAsync(
        string            externalPaymentId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Creates a return to the provider.
    /// </summary>
    Task<CreateProviderRefundResult> RefundAsync(
        CreateProviderRefundRequest request,
        CancellationToken           cancellationToken);
}

