using RealEstateAgencyApp.Domain.Entities;

namespace RealEstateAgencyApp.Domain.Interfaces;

/// <summary>
/// Repository for managing Counterparty entities in the data storage.
/// Provides CRUD operations and specific queries for counterparties.
/// </summary>
public interface ICounterpartyRepository
{
    /// <summary>
    /// Retrieves all counterparties from the database.
    /// </summary>
    /// <returns>A collection of all counterparties.</returns>
    public Task<IEnumerable<Counterparty>> GetAllAsync();

    /// <summary>
    /// Retrieves a counterparty by its unique identifier.
    /// </summary>
    /// <param name="id">The counterparty identifier.</param>
    /// <returns>The counterparty if found; otherwise, null.</returns>
    public Task<Counterparty?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves a counterparty by passport number.
    /// </summary>
    /// <param name="passportNumber">The passport number to search for.</param>
    /// <returns>The counterparty if found; otherwise, null.</returns>
    public Task<Counterparty?> GetByPassportNumberAsync(string passportNumber);

    /// <summary>
    /// Checks if a counterparty exists with the specified identifier.
    /// </summary>
    /// <param name="id">The counterparty identifier to check.</param>
    /// <returns>True if a counterparty exists; otherwise, false.</returns>
    public Task<bool> ExistsByIdAsync(int id);

    /// <summary>
    /// Checks if a counterparty exists with the specified passport number.
    /// </summary>
    /// <param name="passportNumber">The passport number to check.</param>
    /// <returns>True if a counterparty exists; otherwise, false.</returns>
    public Task<bool> ExistsByPassportNumberAsync(string passportNumber);

    /// <summary>
    /// Adds a new counterparty to the database.
    /// </summary>
    /// <param name="counterparty">The counterparty to add.</param>
    public Task AddAsync(Counterparty counterparty);

    /// <summary>
    /// Updates an existing counterparty in the database.
    /// </summary>
    /// <param name="counterparty">The counterparty with updated data.</param>
    public Task UpdateAsync(Counterparty counterparty);

    /// <summary>
    /// Deletes a counterparty by its identifier.
    /// </summary>
    /// <param name="id">The counterparty identifier to delete.</param>
    public Task DeleteAsync(int id);
}