namespace RealEstateAgencyApp.Application.Dtos.AnalyticsDtos;

/// <summary>
/// Represents a DTO for monthly request statistics.
/// </summary>
public class MonthlyRequestsDto
{
    /// <summary>
    /// Year and month (e.g., "2024-01").
    /// </summary>
    public required string Month { get; set; }

    /// <summary>
    /// Number of buy requests.
    /// </summary>
    public required int BuyCount { get; set; }

    /// <summary>
    /// Number of sell requests.
    /// </summary>
    public required int SellCount { get; set; }

    /// <summary>
    /// Total number of requests.
    /// </summary>
    public required int TotalCount { get; set; }

    /// <summary>
    /// Total value of all requests.
    /// </summary>
    public required decimal TotalValue { get; set; }
}