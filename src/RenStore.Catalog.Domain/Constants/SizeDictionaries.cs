using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.Constants;

/// <summary>
/// Represents a dictionary containing size mappings.
/// </summary>
internal static class SizeDictionaries
{
    /// <summary>
    /// Clothing sizes in the Russian size chart.
    /// </summary>
    public static readonly Dictionary<LetterSize, decimal> ClothSizesRu = new()
    {
        { LetterSize.XXS, 40 },
        { LetterSize.XS, 42 },
        { LetterSize.S, 44 },
        { LetterSize.M, 46 },
        { LetterSize.L, 48 },
        { LetterSize.XL, 50 },
        { LetterSize.XXL, 52 }
    };
    
    /// <summary>
    /// Clothing sizes in the European size chart.
    /// </summary>
    public static readonly Dictionary<LetterSize, decimal> ClothSizesEu = new()
    {
        { LetterSize.XXS, 32 },
        { LetterSize.XS, 34 },
        { LetterSize.S, 36 },
        { LetterSize.M, 38 },
        { LetterSize.L, 40 },
        { LetterSize.XL, 42 },
        { LetterSize.XXL, 44 }
    };
    
    /// <summary>
    /// Clothing sizes in the American size chart.
    /// </summary>
    public static readonly Dictionary<LetterSize, decimal> ClothSizesUs = new()
    {
        { LetterSize.XXS, 2 },
        { LetterSize.XS, 4 },
        { LetterSize.S, 6 },
        { LetterSize.M, 8 },
        { LetterSize.L, 10 },
        { LetterSize.XL, 12 },
        { LetterSize.XXL, 14 }
    };
}