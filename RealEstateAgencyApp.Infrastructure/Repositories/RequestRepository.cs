using Microsoft.EntityFrameworkCore;
using RealEstateAgencyApp.Domain.Entities;
using RealEstateAgencyApp.Domain.Entities.Enums;
using RealEstateAgencyApp.Domain.Interfaces;
using RealEstateAgencyApp.Infrastructure.Persistence;

namespace RealEstateAgencyApp.Infrastructure.Repositories;

/// <summary>
/// Repository for managing Request entities.
/// Provides CRUD methods for requests.
/// </summary>
/// <param name="context">The application's database context used for data access.</param>
public class RequestRepository(DBContext context) : IRequestRepository
{
    /// <summary>
    /// Gets all requests.
    /// </summary>
    public async Task<IEnumerable<Request>> GetAllAsync() =>
        await context.Requests.ToListAsync();

    /// <summary>
    /// Gets a request by its ID.
    /// </summary>
    /// <param name="id">Request ID.</param>
    /// <returns>The <see cref="Request"/> if found; otherwise, null.</returns>
    public async Task<Request?> GetByIdAsync(int id) =>
        await context.Requests.FirstOrDefaultAsync(r => r.Id == id);

    /// <summary>
    /// Gets requests by counterparty ID.
    /// </summary>
    /// <param name="counterpartyId">Counterparty ID.</param>
    /// <returns>List of requests for specified counterparty.</returns>
    public async Task<IEnumerable<Request>> GetByCounterpartyIdAsync(int counterpartyId) =>
        await context.Requests
            .Where(r => r.CounterpartyID == counterpartyId)
            .ToListAsync();

    /// <summary>
    /// Gets requests by estate ID.
    /// </summary>
    /// <param name="estateId">Estate ID.</param>
    /// <returns>List of requests for specified estate.</returns>
    public async Task<IEnumerable<Request>> GetByEstateIdAsync(int estateId) =>
        await context.Requests
            .Where(r => r.EstateID == estateId)
            .ToListAsync();

    /// <summary>
    /// Gets requests by type.
    /// </summary>
    /// <param name="type">Type of request.</param>
    /// <returns>List of requests of specified type.</returns>
    public async Task<IEnumerable<Request>> GetByTypeAsync(RequestType type) =>
        await context.Requests
            .Where(r => r.Type == type)
            .ToListAsync();

    /// <summary>
    /// Gets requests by date range.
    /// </summary>
    /// <param name="startDate">Start date.</param>
    /// <param name="endDate">End date.</param>
    /// <returns>List of requests within specified date range.</returns>
    public async Task<IEnumerable<Request>> GetByDateRangeAsync(DateTime startDate, DateTime endDate) =>
        await context.Requests
            .Where(r => r.Date >= startDate && r.Date <= endDate)
            .ToListAsync();

    /// <summary>
    /// Checks if a request exists by ID.
    /// </summary>
    /// <param name="id">Request ID.</param>
    /// <returns>True if exists; otherwise, false.</returns>
    public async Task<bool> ExistsByIdAsync(int id) =>
        await context.Requests.AnyAsync(r => r.Id == id);

    /// <summary>
    /// Adds a new request.
    /// </summary>
    /// <param name="request">Request to add.</param>
    public async Task AddAsync(Request request)
    {
        await context.Requests.AddAsync(request);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing request.
    /// </summary>
    /// <param name="request">Request with updated data.</param>
    public async Task UpdateAsync(Request request)
    {
        var existingRequest = await context.Requests.FindAsync(request.Id) ??
            throw new KeyNotFoundException($"Request with Id {request.Id} not found.");

        existingRequest.CounterpartyID = request.CounterpartyID;
        existingRequest.EstateID = request.EstateID;
        existingRequest.Type = request.Type;
        existingRequest.Price = request.Price;
        existingRequest.Date = request.Date;

        context.Requests.Update(existingRequest);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a request by ID.
    /// </summary>
    /// <param name="id">Request ID.</param>
    public async Task DeleteAsync(int id)
    {
        var request = await context.Requests.FindAsync(id) ??
            throw new KeyNotFoundException($"Request with Id {id} not found.");

        context.Requests.Remove(request);
        await context.SaveChangesAsync();
    }

}