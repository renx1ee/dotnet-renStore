namespace RenStore.Delivery.Domain.ValueObjects;

public class WeightLimitKg
{
    public decimal Kilograms { get; }
    
    private WeightLimitKg() { }

    public WeightLimitKg(decimal kilograms)
    {
        if (kilograms < 0)
            throw new InvalidOperationException("Weight cannot be less 0.");
        Kilograms = kilograms;
    }

    public WeightLimitKg Change(decimal newKilograms) => new WeightLimitKg(newKilograms);
}