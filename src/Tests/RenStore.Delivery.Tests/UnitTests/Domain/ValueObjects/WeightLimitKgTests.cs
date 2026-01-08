using RenStore.Delivery.Domain.ValueObjects;

namespace RenStore.Delivery.Tests.UnitTests.Domain.ValueObjects;

public sealed class WeightLimitKgTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(1.01)]
    [InlineData(1000)]
    public async Task Constructor(decimal kilograms)
    {
        var result = new WeightLimitKg(kilograms);
        
        Assert.Equal(result.Kilograms, kilograms);
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(-0.1)]
    [InlineData(-100)]
    [InlineData(-999.00)]
    [InlineData(1001)]
    public async Task Create_WeightLimitKg_Failure_Test(decimal kilograms)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new WeightLimitKg(kilograms));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(0.1)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(999.00)]
    [InlineData(1000)]
    public async Task Create_WeightLimitKg_Success_Test(decimal kilograms)
    {
        var result = new WeightLimitKg(kilograms);

        Assert.NotNull(result);
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(-0.1)]
    [InlineData(-100)]
    [InlineData(-999.00)]
    [InlineData(1001)]
    [InlineData(1001.01)]
    [InlineData(10000)]
    public async Task Change_WeightLimitKg_Failure_Test(decimal kilograms)
    {
        var result = new WeightLimitKg(1);
        
        Assert.Throws<ArgumentOutOfRangeException>(() => result.Change(kilograms));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(0.1)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(999.00)]
    [InlineData(1000)]
    public async Task Change_WeightLimitKg_Success_Test(decimal kilograms)
    {
        var weightLimitKg = new WeightLimitKg(kilograms);

        var result = weightLimitKg.Change(kilograms);

        Assert.NotNull(result);
    }
}