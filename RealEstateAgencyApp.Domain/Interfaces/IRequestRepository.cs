using RealEstateAgencyApp.Domain.Entities;
using RealEstateAgencyApp.Domain.Entities.Enums;

namespace RealEstateAgencyApp.Domain.Interfaces;

/// <summary>
/// Repository for managing Request entities in the data storage.
/// Provides CRUD operations and specific queries for real estate requests.
/// </summary>
public interface IRequestRepository
{
    /// <summary>
    /// Retrieves all requests from the database.
    /// </summary>
    /// <returns>A collection of all requests.</returns>
    public Task<IEnumerable<Request>> GetAllAsync();

    /// <summary>
    /// Retrieves a request by its unique identifier.
    /// </summary>
    /// <param name="id">The request identifier.</param>
    /// <returns>The request if found; otherwise, null.</returns>
    public Task<Request?> GetByIdAsync(int id);

    /// <summary>
    /// Retrieves requests by counterparty identifier.
    /// </summary>
    /// <param name="counterpartyId">The counterparty identifier to filter by.</param>
    /// <returns>A collection of requests for the specified counterparty.</returns>
    public Task<IEnumerable<Request>> GetByCounterpartyIdAsync(int counterpartyId);

    /// <summary>
    /// Retrieves requests by real estate object identifier.
    /// </summary>
    /// <param name="estateId">The real estate object identifier to filter by.</param>
    /// <returns>A collection of requests for the specified real estate object.</returns>
    public Task<IEnumerable<Request>> GetByEstateIdAsync(int estateId);

    /// <summary>
    /// Retrieves requests by request type.
    /// </summary>
    /// <param name="type">The request type to filter by.</param>
    /// <returns>A collection of requests of the specified type.</returns>
    public Task<IEnumerable<Request>> GetByTypeAsync(RequestType type);

    /// <summary>
    /// Retrieves requests within a specified date range.
    /// </summary>
    /// <param name="startDate">The start date of the range.</param>
    /// <param name="endDate">The end date of the range.</param>
    /// <returns>A collection of requests within the specified date range.</returns>
    public Task<IEnumerable<Request>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Checks if a request exists with the specified identifier.
    /// </summary>
    /// <param name="id">The request identifier to check.</param>
    /// <returns>True if a request exists; otherwise, false.</returns>
    public Task<bool> ExistsByIdAsync(int id);

    /// <summary>
    /// Adds a new request to the database.
    /// </summary>
    /// <param name="request">The request to add.</param>
    public Task AddAsync(Request request);

    /// <summary>
    /// Updates an existing request in the database.
    /// </summary>
    /// <param name="request">The request with updated data.</param>
    public Task UpdateAsync(Request request);

    /// <summary>
    /// Deletes a request by its identifier.
    /// </summary>
    /// <param name="id">The request identifier to delete.</param>
    public Task DeleteAsync(int id);
}