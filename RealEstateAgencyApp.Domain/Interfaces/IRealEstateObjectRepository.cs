using RealEstateAgencyApp.Domain.Entities;
using RealEstateAgencyApp.Domain.Entities.Enums;

namespace RealEstateAgencyApp.Domain.Interfaces;

/// <summary>
/// Repository for managing RealEstateObject entities in the data storage.
/// Provides CRUD operations and specific queries for real estate objects.
/// </summary>
public interface IRealEstateObjectRepository
{
    /// <summary>
    /// Retrieves all real estate objects from the database.
    /// </summary>
    /// <returns>A collection of all real estate objects.</returns>
    public Task<IEnumerable<RealEstateObject>> GetAllAsync();

    /// <summary>
    /// Retrieves a real estate object by its unique identifier.
    /// </summary>
    /// <param name="id">The real estate object identifier.</param>
    /// <returns>The real estate object if found; otherwise, null.</returns>
    public Task<RealEstateObject?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves a real estate object by cadastral number.
    /// </summary>
    /// <param name="cadastralNumber">The cadastral number to search for.</param>
    /// <returns>The real estate object if found; otherwise, null.</returns>
    public Task<RealEstateObject?> GetByCadastralNumberAsync(string cadastralNumber);

    /// <summary>
    /// Retrieves real estate objects by their type.
    /// </summary>
    /// <param name="type">The type of real estate to filter by.</param>
    /// <returns>A collection of real estate objects of the specified type.</returns>
    public Task<IEnumerable<RealEstateObject>> GetByTypeAsync(RealEstateType type);

    /// <summary>
    /// Retrieves real estate objects by their purpose.
    /// </summary>
    /// <param name="purpose">The purpose of real estate to filter by.</param>
    /// <returns>A collection of real estate objects of the specified purpose.</returns>
    public Task<IEnumerable<RealEstateObject>> GetByPurposeAsync(RealEstatePurpose purpose);

    /// <summary>
    /// Checks if a real estate object exists with the specified identifier.
    /// </summary>
    /// <param name="id">The real estate object identifier to check.</param>
    /// <returns>True if a real estate object exists; otherwise, false.</returns>
    public Task<bool> ExistsByIdAsync(int id);

    /// <summary>
    /// Checks if a real estate object exists with the specified cadastral number.
    /// </summary>
    /// <param name="cadastralNumber">The cadastral number to check.</param>
    /// <returns>True if a real estate object exists; otherwise, false.</returns>
    public Task<bool> ExistsByCadastralNumberAsync(string cadastralNumber);

    /// <summary>
    /// Adds a new real estate object to the database.
    /// </summary>
    /// <param name="realEstateObject">The real estate object to add.</param>
    public Task AddAsync(RealEstateObject realEstateObject);

    /// <summary>
    /// Updates an existing real estate object in the database.
    /// </summary>
    /// <param name="realEstateObject">The real estate object with updated data.</param>
    public Task UpdateAsync(RealEstateObject realEstateObject);

    /// <summary>
    /// Deletes a real estate object by its identifier.
    /// </summary>
    /// <param name="id">The real estate object identifier to delete.</param>
    public Task DeleteAsync(int id);
}