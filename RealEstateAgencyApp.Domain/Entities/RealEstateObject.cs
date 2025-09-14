using RealEstateAgencyApp.Domain.Entities.Enums;

namespace RealEstateAgencyApp.Domain.Entities;

/// <summary>
/// Represents a real estate property with its characteristics.
/// </summary>
public class RealEstateObject
{
    public required int Id { get; set; }

    public required RealEstateType Type { get; set; }
    public required RealEstatePurpose Purpose { get; set; }

    public required string CadastralNumber { get; set; }
    public required string Address { get; set; }

    public required int Floors { get; set; }
    public required double Area { get; set; }
    public required int Rooms { get; set; }
    public required double CeilingHeight { get; set; } 
    public required int Floor { get; set; }         
    public required bool HasEncumbrances { get; set; }

}
