using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.SharedKernal.Domain.ValueObjects;

public sealed class Price
{
    public decimal Amount { get; }
    public Currency Currency { get; }

    public Price(decimal amount, Currency currency)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount), 
                "Amount cannot be negative 0.");
        
        Amount = amount;
        Currency = currency;
    }

    public Price Add(Price other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException(
                "Cannot add prices with different currencies.");

        return new Price(Amount + other.Amount, Currency);
    }

    public Price Subtract(Price other)
    {
        if (other.Currency != Currency)
            throw new InvalidOperationException(
                "Cannot subtract prices with different currencies.");

        var result = Amount - other.Amount;
        
        if(result < 0)
            throw new InvalidOperationException(
                "Price cannot be negative.");

        return new Price(result, Currency);
    }
    
    public static void ValidateNewPrice(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount), 
                "Amount cannot be negative 0.");
    }
}