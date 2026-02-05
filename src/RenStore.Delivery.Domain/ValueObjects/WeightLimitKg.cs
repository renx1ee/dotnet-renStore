namespace RenStore.Delivery.Domain.ValueObjects;

public sealed class WeightLimitKg
{
    private const decimal MaxWeight = 1000m;
    private const decimal MinWeight = 0m;
    
    /// <summary>
    /// Min 0, Max 1000.
    /// </summary>
    public decimal Kilograms { get; }
    
    private WeightLimitKg() { }

    public WeightLimitKg(decimal kilograms)
    {
        if (kilograms < MinWeight || kilograms > MaxWeight)
            throw new ArgumentOutOfRangeException(
                nameof(kilograms),
                $"Weight must be between {MinWeight} and {MaxWeight} kg.");
        
        Kilograms = kilograms;
    }

    public WeightLimitKg Change(decimal newKilograms) => new WeightLimitKg(newKilograms);
}