namespace RealEstateAgencyApp.Application.Dtos.RequestDtos;

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
    /// ID of the counterparty making the request.
    /// </summary>
    public required int CounterpartyID { get; set; }

    /// <summary>
    /// ID of the real estate object related to the request.
    /// </summary>
    public required int EstateID { get; set; }

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