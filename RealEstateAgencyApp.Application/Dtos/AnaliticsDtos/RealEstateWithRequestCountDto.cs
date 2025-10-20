namespace RealEstateAgencyApp.Application.Dtos.AnalyticsDtos;

/// <summary>
/// Represents a DTO for a real estate object with request count.
/// </summary>
public class RealEstateWithRequestCountDto
{
    /// <summary>
    /// Unique identifier of the real estate object.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Type of the real estate object.
    /// </summary>
    public required RealEstateType Type { get; set; }

    /// <summary>
    /// Address of the property.
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Area of the property in square meters.
    /// </summary>
    public required double Area { get; set; }

    /// <summary>
    /// Number of requests for this property.
    /// </summary>
    public required int RequestCount { get; set; }
}