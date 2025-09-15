using RealEstateAgencyApp.Domain.Entities.Enums;

namespace RealEstateAgencyApp.Domain.Entities;

/// <summary>
/// Represents a real estate property with its characteristics.
/// </summary>
public class RealEstateObject
{
    /// <summary>
    /// Gets or sets the unique identifier of the real estate object.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Gets or sets the type of the real estate object.
    /// </summary>
    public required RealEstateType Type { get; set; }

    /// <summary>
    /// Gets or sets the purpose of the real estate object.
    /// </summary>
    public required RealEstatePurpose Purpose { get; set; }

    /// <summary>
    /// Gets or sets the cadastral number of the property.
    /// </summary>
    public required string CadastralNumber { get; set; }

    /// <summary>
    /// Gets or sets the physical address of the property.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Gets or sets the total number of floors in the building.
    /// </summary>
    public int? Floors { get; set; }

    /// <summary>
    /// Gets or sets the area of the property in square meters.
    /// </summary>
    public required double Area { get; set; }

    /// <summary>
    /// Gets or sets the number of rooms in the property.
    /// </summary>
    public int? Rooms { get; set; }

    /// <summary>
    /// Gets or sets the ceiling height in meters.
    /// </summary>
    public double? CeilingHeight { get; set; }

    /// <summary>
    /// Gets or sets the floor number where the property is located.
    /// </summary>
    public int? Floor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the property has any legal encumbrances.
    /// </summary>
    public bool? HasEncumbrances { get; set; }
}