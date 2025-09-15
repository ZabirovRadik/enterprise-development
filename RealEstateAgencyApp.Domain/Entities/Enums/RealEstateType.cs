namespace RealEstateAgencyApp.Domain.Entities.Enums;

/// <summary>
/// Type of real estate property.
/// </summary>
public enum RealEstateType
{
    /// <summary>
    /// An apartment unit in a multi-story building.
    /// </summary>
    Apartment,

    /// <summary>
    /// A standalone residential house.
    /// </summary>
    House,

    /// <summary>
    /// A commercial office space.
    /// </summary>
    Office,

    /// <summary>
    /// A plot of land without buildings.
    /// </summary>
    Land,

    /// <summary>
    /// A garage for vehicle storage.
    /// </summary>
    Garage
}
