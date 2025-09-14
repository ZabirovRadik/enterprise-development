using RealEstateAgencyApp.Domain.Entities.Enums;

namespace RealEstateAgencyApp.Domain.Entities;

/// <summary>
/// Represents a request for buying or selling real estate.
/// </summary>
public class Request
{
    public required int Id { get; set; }

    public required int CounterpartyId { get; set; }
    public Counterparty? Counterparty { get; set; }

    public required int RealEstateObjectId { get; set; }
    public RealEstateObject? Estate { get; set; }

    public required RequestType Type { get; set; }
    public required decimal Price { get; set; }
    public required DateTime Date { get; set; }
}
