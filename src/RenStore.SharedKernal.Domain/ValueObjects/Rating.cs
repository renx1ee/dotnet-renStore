using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.SharedKernal.Domain.ValueObjects;

/// <summary>
/// Value Object for working with Rating.
/// </summary>
public sealed class Rating
{
    /// <summary>
    /// Total number of ratings.
    /// (Общее количество оценок.)
    /// </summary>
    public int TotalRatings { get; }
    
    /// <summary>
    /// Sum of all ratings.
    /// (Сумма всех оценок.)
    /// </summary>
    public decimal TotalScore { get; }

    /// <summary>
    /// Average rating value from total scores and ratings count.
    /// (Вычисляемый итоговый рейтинг.)
    /// </summary>
    public decimal Value => TotalRatings == 0
        ? 0
        : TotalScore / TotalRatings;
    
    private const decimal MinRating = 0;
    private const decimal MaxRating = 5;

    private Rating(
        int totalRatings,
        decimal totalScore)
    {
        if (totalRatings < 0)
            throw new DomainException("Total ratings count cannot be negative.");
        
        if (totalScore < 0)
            throw new DomainException("Total score cannot be negative.");

        if (totalRatings > 0)
        {
            var value = totalScore / totalRatings;
            if(value is > MaxRating or < MinRating)
                throw new DomainException($"Rating must be between {MinRating} and {MaxRating}.");
        }
        
        TotalRatings = totalRatings;
        TotalScore = totalScore;
    }
    
    /// <summary>
    /// Add a rating score to an existing rating.
    /// </summary>
    /// <param name="score">Rating value.</param>
    /// <returns></returns>
    /// <exception cref="DomainException">Throws if score less than <see cref="MinRating"/> or more then <see cref="MaxRating"/>.</exception>
    public Rating Add(decimal score)
    {
        if(score is > MaxRating or < MinRating)
            throw new DomainException($"Score must be between {MinRating} and {MaxRating}.");

        return new Rating(
            totalRatings: TotalRatings + 1,
            totalScore: TotalScore + score);
    }

    /// <summary>
    /// Creates an Empty Rating.
    /// </summary>
    public static Rating Empty() => 
        new Rating(0, 0);

    /// <summary>
    /// Reconstitute rating from persisted state.
    /// Intended for restoring state, not for normal creation.
    /// </summary>
    /// <param name="totalRatings">Total Ratings.</param>
    /// <param name="totalScore">Total Scores.</param>
    /// <returns>Restored rating.</returns>
    public static Rating From(int totalRatings, decimal totalScore) => 
        new Rating(totalRatings: totalRatings, 
                   totalScore: totalScore);

    public override bool Equals(object? obj) 
        => obj is Rating other
           && TotalRatings == other.TotalRatings
           && TotalScore == other.TotalScore;
}