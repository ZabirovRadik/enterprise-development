using RealEstateAgencyApp.Domain.Entities;
using RealEstateAgencyApp.Domain.Entities.Enums;

namespace RealEstateAgencyApp.Contracts.Dtos.RequestDtos;

/// <summary>
/// Represents a DTO for creating or updating a request.
/// </summary>
public class RequestEditDto
{
    /// <summary>
    /// Gets or sets the counterparty making the request.
    /// </summary>
    public required Counterparty Counterparty { get; set; }

    /// <summary>
    /// Gets or sets the real estate object related to the request.
    /// </summary>
    public required RealEstateObject Estate { get; set; }

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