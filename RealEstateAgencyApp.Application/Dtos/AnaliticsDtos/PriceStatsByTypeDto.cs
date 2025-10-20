namespace RealEstateAgencyApp.Application.Dtos.AnalyticsDtos;

/// <summary>
/// Represents a DTO for price statistics by real estate type.
/// </summary>
public class PriceStatsByTypeDto
{
    /// <summary>
    /// Type of real estate.
    /// </summary>
    public required RealEstateType Type { get; set; }

    /// <summary>
    /// Average price for this type.
    /// </summary>
    public required decimal AveragePrice { get; set; }

    /// <summary>
    /// Maximum price for this type.
    /// </summary>
    public required decimal MaxPrice { get; set; }

    /// <summary>
    /// Minimum price for this type.
    /// </summary>
    public required decimal MinPrice { get; set; }

    /// <summary>
    /// Total number of requests for this type.
    /// </summary>
    public required int RequestCount { get; set; }
}