using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RenStore.Payment.Application.Abstractions.Services;
using RenStore.Payment.Application.Contracts.Requests;
using RenStore.Payment.Application.Options;

namespace RenStore.Payment.Application.Services;

public sealed class YooKassaPaymentService(
    HttpClient                    httpClient,
    IOptions<YooKassaOptions>     options)
    : IPaymentProviderService
{
    private readonly YooKassaOptions _options = options.Value;

    public async Task<CreateProviderPaymentResult> CreateAsync(
        CreateProviderPaymentRequest request,
        CancellationToken            cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Remove("Idempotence-Key");
        httpClient.DefaultRequestHeaders.Add("Idempotence-Key", request.PaymentId.ToString());

        var body = new
        {
            amount = new
            {
                value    = request.Amount.ToString("F2"),
                currency = request.Currency
            },
            confirmation = new
            {
                type       = "redirect",
                return_url = request.ReturnUrl
            },
            capture     = false, // двухстадийная оплата: сначала авторизация, потом capture
            description = request.Description
        };

        var content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            mediaType: "application/json");

        var response = await httpClient.PostAsync(requestUri: "payments", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        return new CreateProviderPaymentResult(
            ExternalPaymentId: root.GetProperty("id").GetString()!,
            ConfirmationUrl:   root
                .GetProperty("confirmation")
                .GetProperty("confirmation_url")
                .GetString()!,
            IsSucceeded: root.GetProperty("status").GetString() != "canceled");
    }

    public async Task<bool> CaptureAsync(
        string            externalPaymentId,
        CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Remove("Idempotence-Key");
        httpClient.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());

        var response = await httpClient.PostAsync(
            requestUri: $"payments/{externalPaymentId}/capture",
            content: null,
            cancellationToken);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CancelAsync(
        string            externalPaymentId,
        CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Remove("Idempotence-Key");
        httpClient.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());

        var response = await httpClient.PostAsync(
            requestUri: $"payments/{externalPaymentId}/cancel",
            content: null,
            cancellationToken);

        return response.IsSuccessStatusCode;
    }

    public async Task<CreateProviderRefundResult> RefundAsync(
        CreateProviderRefundRequest request,
        CancellationToken           cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Remove("Idempotence-Key");
        httpClient.DefaultRequestHeaders.Add("Idempotence-Key", Guid.NewGuid().ToString());

        var body = new
        {
            payment_id = request.ExternalPaymentId,
            amount = new
            {
                value    = request.Amount.ToString("F2"),
                currency = request.Currency
            },
            description = request.Reason
        };

        var content = new StringContent(
            JsonSerializer.Serialize(body),
            Encoding.UTF8,
            mediaType: "application/json");

        var response = await httpClient.PostAsync("refunds", content, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(json);

        return new CreateProviderRefundResult(
            ExternalRefundId: doc.RootElement.GetProperty("id").GetString()!,
            IsSucceeded:      doc.RootElement.GetProperty("status").GetString() == "succeeded");
    }
}