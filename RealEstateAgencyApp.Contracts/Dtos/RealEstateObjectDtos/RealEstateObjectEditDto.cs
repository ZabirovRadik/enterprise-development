using RealEstateAgencyApp.Domain.Entities.Enums;

namespace RealEstateAgencyApp.Contracts.Dtos.RealEstateObjectDtos;

/// <summary>
/// Represents a DTO for creating or updating a real estate object.
/// </summary>
public class RealEstateObjectEditDto
{
    /// <summary>
    /// Type of the real estate object.
    /// </summary>
    public required RealEstateType Type { get; set; }

    /// <summary>
    /// Purpose of the real estate object.
    /// </summary>
    public required RealEstatePurpose Purpose { get; set; }

    /// <summary>
    /// Cadastral number of the property.
    /// </summary>
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Physical address of the property.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Total number of floors in the building.
    /// </summary>
    public int? Floors { get; set; }

    /// <summary>
    /// Area of the property in square meters.
    /// </summary>
    public required double Area { get; set; }

    /// <summary>
    /// Number of rooms in the property.
    /// </summary>
    public int? Rooms { get; set; }

    /// <summary>
    /// Ceiling height in meters.
    /// </summary>
    public double? CeilingHeight { get; set; }

    /// <summary>
    /// Floor number where the property is located.
    /// </summary>
    public int? Floor { get; set; }

    /// <summary>
    /// Indicates whether the property has any legal encumbrances.
    /// </summary>
    public bool? HasEncumbrances { get; set; }
}