// Domain/Entities/DeliveryTariff.cs
using RenStore.Delivery.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Entities;

/// <summary>
/// Тариф доставки. Справочная сущность.
/// Для Почты России тип = RussianPost.
/// </summary>
public sealed class DeliveryTariff
{
    public int                Id            { get; private set; }
    public decimal            PriceAmount   { get; private set; }
    public string             Currency      { get; private set; } = string.Empty;
    public decimal            WeightLimitKg { get; private set; }
    public DeliveryTariffType Type          { get; private set; }
    public string             Description   { get; private set; } = string.Empty;
    public bool               IsDeleted     { get; private set; }

    public DateTimeOffset  CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    private DeliveryTariff() { }

    public static DeliveryTariff Create(
        decimal            priceAmount,
        string             currency,
        decimal            weightLimitKg,
        DeliveryTariffType type,
        string             description,
        DateTimeOffset     now)
    {
        if (priceAmount < 0)
            throw new DomainException("Price cannot be negative.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency cannot be empty.");

        if (weightLimitKg <= 0)
            throw new DomainException("WeightLimitKg must be greater than 0.");

        return new DeliveryTariff
        {
            PriceAmount   = priceAmount,
            Currency      = currency.Trim().ToUpperInvariant(),
            WeightLimitKg = weightLimitKg,
            Type          = type,
            Description   = description?.Trim() ?? string.Empty,
            IsDeleted     = false,
            CreatedAt     = now
        };
    }

    public void ChangePrice(decimal priceAmount, string currency, DateTimeOffset now)
    {
        EnsureNotDeleted();

        if (priceAmount < 0)
            throw new DomainException("Price cannot be negative.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency cannot be empty.");

        PriceAmount = priceAmount;
        Currency    = currency.Trim().ToUpperInvariant();
        UpdatedAt   = now;
    }

    public void ChangeWeightLimit(decimal weightLimitKg, DateTimeOffset now)
    {
        EnsureNotDeleted();

        if (weightLimitKg <= 0)
            throw new DomainException("WeightLimitKg must be greater than 0.");

        WeightLimitKg = weightLimitKg;
        UpdatedAt     = now;
    }

    public void ChangeDescription(string description, DateTimeOffset now)
    {
        EnsureNotDeleted();

        Description = description?.Trim() ?? string.Empty;
        UpdatedAt   = now;
    }

    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted();

        IsDeleted = true;
        DeletedAt = now;
        UpdatedAt = now;
    }

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new DomainException("Cannot modify deleted tariff.");
    }
}