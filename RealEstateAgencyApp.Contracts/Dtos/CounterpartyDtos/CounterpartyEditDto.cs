namespace RealEstateAgencyApp.Contracts.Dtos.CounterpartyDtos;

/// <summary>
/// Represents a DTO for creating or updating a counterparty.
/// </summary>
public class CounterpartyEditDto
{
    /// <summary>
    /// Full name of the counterparty.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Passport number of the counterparty.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Phone number of the counterparty.
    /// </summary>
    public required string Phone { get; set; }
}