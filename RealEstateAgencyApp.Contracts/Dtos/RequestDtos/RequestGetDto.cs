using RealEstateAgencyApp.Domain.Entities;
using RealEstateAgencyApp.Domain.Entities.Enums;

namespace RealEstateAgencyApp.Contracts.Dtos.RequestDtos;

/// <summary>
/// Represents a DTO for retrieving request information.
/// </summary>
public class RequestGetDto
{
    /// <summary>
    /// Unique identifier of the request.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Gets or sets the counterparty identifier making the request.
    /// </summary>
    public required int CounterpartyId { get; set; }

    /// <summary>
    /// Gets or sets the full name of the counterparty.
    /// </summary>
    public required string CounterpartyName { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the counterparty.
    /// </summary>
    public required string CounterpartyPhone { get; set; }

    /// <summary>
    /// Gets or sets the real estate object identifier related to the request.
    /// </summary>
    public required int EstateId { get; set; }

    /// <summary>
    /// Gets or sets the address of the real estate object.
    /// </summary>
    public required string EstateAddress { get; set; }

    /// <summary>
    /// Gets or sets the cadastral number of the real estate object.
    /// </summary>
    public required string EstateCadastralNumber { get; set; }

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