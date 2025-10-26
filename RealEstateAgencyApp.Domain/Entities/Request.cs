using RealEstateAgencyApp.Domain.Entities.Enums;

namespace RealEstateAgencyApp.Domain.Entities;

/// <summary>
/// Represents a request for buying or selling real estate.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier of the request.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Gets or sets the counterparty making the request.
    /// </summary>
    public required Counterparty Counterparty { get; set; }

    /// <summary>
    /// Gets or sets the real estate object related to the request.
    /// </summary>
    public required RealEstateObject Estate { get; set; }

    /// <summary>
    /// Gets or sets the type of the request (buy or sell).
    /// </summary>
    public required RequestType Type { get; set; }

    /// <summary>
    /// Gets or sets the price proposed in the request.
    /// </summary>
    public required decimal Price { get; set; }

    /// <summary>
    /// Gets or sets the date when the request was created.
    /// </summary>
    public required DateTime Date { get; set; }
}
