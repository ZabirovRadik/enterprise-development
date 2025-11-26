using RealEstateAgencyApp.Domain.Entities.Enums;

namespace RealEstateAgencyApp.Contracts.Dtos.RequestDtos;

/// <summary>
/// Represents a DTO for creating a request via API.
/// </summary>
public class RequestCreateDto
{
    /// <summary>
    /// Gets or sets the counterparty identifier.
    /// </summary>
    public required int CounterpartyId { get; set; }

    /// <summary>
    /// Gets or sets the real estate object identifier.
    /// </summary>
    public required int EstateId { get; set; }

    /// <summary>
    /// Type of the request (buy or sell).
    /// </summary>
    public required RequestType Type { get; set; }

    /// <summary>
    /// Price proposed in the request.
    /// </summary>
    public required decimal Price { get; set; }

    /// <summary>
    /// Date when the request was created.
    /// </summary>
    public required DateTime Date { get; set; }
}