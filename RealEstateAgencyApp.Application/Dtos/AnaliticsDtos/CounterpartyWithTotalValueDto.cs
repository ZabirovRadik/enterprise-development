namespace RealEstateAgencyApp.Application.Dtos.AnalyticsDtos;

/// <summary>
/// Represents a DTO for a counterparty with the total value of their requests.
/// </summary>
public class CounterpartyWithTotalValueDto
{
    /// <summary>
    /// Unique identifier of the counterparty.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Full name of the counterparty.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Phone number of the counterparty.
    /// </summary>
    public required string Phone { get; set; }

    /// <summary>
    /// Total value of all requests from this counterparty.
    /// </summary>
    public required decimal TotalValue { get; set; }
}