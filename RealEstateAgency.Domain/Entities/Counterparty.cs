

namespace RealEstateAgencyApp.Domain.Entities;

/// <summary>
/// Represents a client (counterparty) of the real estate agency.
/// </summary>
public class Counterparty
{
    public required int Id { get; set; }

    public required string FullName { get; set; }
    public required string PassportNumber { get; set; }
    public required string Phone { get; set; }
}
