using RenStore.Payment.Domain.Enums;

namespace RenStore.Payment.Persistence.EntityTypeConfigurations.Conversions;

internal static class PaymentProviderConversion
{
    internal static string PaymentProviderToDatabase(PaymentProvider paymentProvider)
    {
        return paymentProvider switch
        {
            PaymentProvider.SBP      => "sbp",
            PaymentProvider.Stripe   => "stripe",
            PaymentProvider.YooKassa => "yookassa",
            _ => throw new ArgumentOutOfRangeException(nameof(paymentProvider))
        };
    } 
    
    internal static PaymentProvider PaymentProviderFromDatabase(string paymentProvider)
    {
        return paymentProvider switch
        {
            "sbp"      => PaymentProvider.SBP,
            "stripe"   => PaymentProvider.Stripe,
            "yookassa" => PaymentProvider.YooKassa,
            _ => throw new ArgumentOutOfRangeException(nameof(paymentProvider))
        };
    } 
}