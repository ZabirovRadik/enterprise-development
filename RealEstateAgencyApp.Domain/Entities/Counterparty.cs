namespace RealEstateAgencyApp.Domain.Entities;

/// <summary>
/// Represents a client (counterparty) of the real estate agency.
/// </summary>
public class Counterparty
{
    /// <summary>
    /// Gets or sets the unique identifier for the counterparty.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Gets or sets the full name of the counterparty.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Gets or sets the passport number of the counterparty.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the counterparty.
    /// </summary>
    public required string Phone { get; set; }
}
