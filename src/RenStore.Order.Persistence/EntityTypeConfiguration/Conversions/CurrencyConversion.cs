using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Order.Persistence.EntityTypeConfiguration.Conversions;

internal static class CurrencyConversion
{
    internal static string CurrencyToDatabase(Currency currency)
    {
        return currency switch
        {
            Currency.EUR => "eur",
            Currency.RUB => "rub",
            Currency.USD => "usd",
            _ => throw new ArgumentOutOfRangeException(nameof(currency))
        };
    }
    
    internal static Currency CurrencyFromDatabase(string currency)
    {
        return currency switch
        {
            "eur" => Currency.EUR,
            "rub" => Currency.RUB,
            "usd" => Currency.USD,
            _ => throw new ArgumentOutOfRangeException(nameof(currency))
        };
    }
}