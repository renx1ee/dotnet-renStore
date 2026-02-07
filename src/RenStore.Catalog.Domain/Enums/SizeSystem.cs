namespace RenStore.Catalog.Domain.Enums;

/// <summary>
/// Supported size measurement systems for international catalog support.
/// </summary>
public enum SizeSystem
{
    /// <summary>
    /// Russian sizes (RU/Российский)
    /// </summary>
    RU = 0,
    
    /// <summary>
    /// United States sizes (US/Американский)
    /// </summary>
    US = 1,
    
    /// <summary>
    /// European sizes (EU/Европейский).
    /// </summary>
    EU = 2
}