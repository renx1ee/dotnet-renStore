namespace RenStore.Delivery.Domain.ValueObjects;

public class WeightLimitKg
{
    private const decimal MAX_WEIGHT = 1000m;
    private const decimal MIN_WEIGHT = 0m;
    
    /// <summary>
    /// Min 0, Max 1000.
    /// </summary>
    public decimal Kilograms { get; }
    
    private WeightLimitKg() { }

    public WeightLimitKg(decimal kilograms)
    {
        if (kilograms < MIN_WEIGHT || kilograms > MAX_WEIGHT)
            throw new ArgumentOutOfRangeException(
                nameof(kilograms),
                $"Weight must be between {MIN_WEIGHT} and {MAX_WEIGHT} kg.");
        
        Kilograms = kilograms;
    }

    public WeightLimitKg Change(decimal newKilograms) => new WeightLimitKg(newKilograms);
}