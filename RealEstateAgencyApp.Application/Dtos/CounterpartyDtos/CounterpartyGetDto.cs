namespace RealEstateAgencyApp.Application.Dtos.CounterpartyDtos;

/// <summary>
/// Represents a DTO for retrieving counterparty information.
/// </summary>
public class CounterpartyGetDto
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
    /// Passport number of the counterparty.
    /// </summary>
    public required string PassportNumber { get; set; }

    /// <summary>
    /// Phone number of the counterparty.
    /// </summary>
    public required string Phone { get; set; }
}