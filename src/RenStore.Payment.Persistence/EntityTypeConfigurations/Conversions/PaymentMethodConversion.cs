using RenStore.Payment.Domain.Enums;

namespace RenStore.Payment.Persistence.EntityTypeConfigurations.Conversions;

internal static class PaymentMethodConversion
{
    internal static string PaymentMethodToDatabase(PaymentMethod paymentMethod)
    {
        return paymentMethod switch
        {
            PaymentMethod.SBP       => "sbp",
            PaymentMethod.BankCard  => "bank-card",
            PaymentMethod.YooMoney  => "yoo-money",
            PaymentMethod.ApplePay  => "apple-pay",
            PaymentMethod.GooglePay => "google-pay",
            _ => throw new InvalidOperationException(nameof(paymentMethod))
        };
    }
    
    internal static PaymentMethod PaymentMethodFromDatabase(string paymentMethod)
    {
        return paymentMethod switch
        {
              "sbp"        => PaymentMethod.SBP,
              "bank-card"  => PaymentMethod.BankCard,
              "yoo-money"  => PaymentMethod.YooMoney,
              "apple-pay"  => PaymentMethod.ApplePay,
              "google-pay" => PaymentMethod.GooglePay,
            _ => throw new InvalidOperationException(nameof(paymentMethod))
        };
    }
}