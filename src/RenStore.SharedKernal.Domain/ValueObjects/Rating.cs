using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.SharedKernal.Domain.ValueObjects;

public class Rating
{
    private const decimal MinRating = 0;
    private const decimal MaxRating = 5;
    
    /// <summary>
    /// Итоговый рейтинг.
    /// </summary>
    public decimal Value { get; }
    
    /// <summary>
    /// Общее количество оценок.
    /// </summary>
    public int TotalRatings { get; }
    
    /// <summary>
    /// Сумма всех оценок.
    /// </summary>
    public decimal TotalScore { get; }

    private Rating(
        decimal value,
        int totalRatings,
        decimal totalScore)
    {
        Value = value;
        TotalRatings = totalRatings;
        TotalScore = totalScore;
    }

    public static Rating Create(
        decimal value,
        int totalRatings,
        decimal totalScore)
    {
        if (value < MinRating || value > MaxRating)
            throw new DomainException($"Rating must be between {MinRating} and {MaxRating}.");
        
        if(totalScore < 0)
            throw new DomainException("Total score cannot be negative.");

        if (totalRatings > 0)
            value = totalScore / totalRatings;

        return new Rating(
            value: value,
            totalRatings: totalRatings,
            totalScore: totalScore);
    }

    public void Add(
        decimal value,
        int totalRatings,
        decimal totalScore)
    {
        
    }
}