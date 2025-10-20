using Microsoft.EntityFrameworkCore;
using RealEstateAgencyApp.Domain.Entities;
using RealEstateAgencyApp.Domain.Interfaces;
using RealEstateAgencyApp.Infrastructure.Persistence;

namespace RealEstateAgencyApp.Infrastructure.Repositories;

/// <summary>
/// Repository for managing Counterparty entities.
/// Provides CRUD methods for counterparties.
/// </summary>
/// <param name="context">The application's database context used for data access.</param>
public class CounterpartyRepository(DBContext context) : ICounterpartyRepository
{
    /// <summary>
    /// Gets all counterparties.
    /// </summary>
    public async Task<IEnumerable<Counterparty>> GetAllAsync() =>
        await context.Counterparties.ToListAsync();

    /// <summary>
    /// Gets a counterparty by its ID.
    /// </summary>
    /// <param name="id">Counterparty ID.</param>
    /// <returns>The <see cref="Counterparty"/> if found; otherwise, null.</returns>
    public async Task<Counterparty?> GetByIdAsync(int id) =>
        await context.Counterparties.FirstOrDefaultAsync(c => c.Id == id);

    /// <summary>
    /// Gets a counterparty by passport number.
    /// </summary>
    /// <param name="passportNumber">Passport number.</param>
    /// <returns>The <see cref="Counterparty"/> if found; otherwise, null.</returns>
    public async Task<Counterparty?> GetByPassportNumberAsync(string passportNumber) =>
        await context.Counterparties.FirstOrDefaultAsync(c => c.PassportNumber == passportNumber);

    /// <summary>
    /// Checks if a counterparty exists by ID.
    /// </summary>
    /// <param name="id">Counterparty ID.</param>
    /// <returns>True if exists; otherwise, false.</returns>
    public async Task<bool> ExistsByIdAsync(int id) =>
        await context.Counterparties.AnyAsync(c => c.Id == id);

    /// <summary>
    /// Checks if a counterparty exists by passport number.
    /// </summary>
    /// <param name="passportNumber">Passport number.</param>
    /// <returns>True if exists; otherwise, false.</returns>
    public async Task<bool> ExistsByPassportNumberAsync(string passportNumber) =>
        await context.Counterparties.AnyAsync(c => c.PassportNumber == passportNumber);

    /// <summary>
    /// Adds a new counterparty.
    /// </summary>
    /// <param name="counterparty">Counterparty to add.</param>
    public async Task AddAsync(Counterparty counterparty)
    {
        await context.Counterparties.AddAsync(counterparty);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing counterparty.
    /// </summary>
    /// <param name="counterparty">Counterparty with updated data.</param>
    public async Task UpdateAsync(Counterparty counterparty)
    {
        var existingCounterparty = await context.Counterparties.FindAsync(counterparty.Id) ??
            throw new KeyNotFoundException($"Counterparty with Id {counterparty.Id} not found.");

        existingCounterparty.FullName = counterparty.FullName;
        existingCounterparty.PassportNumber = counterparty.PassportNumber;
        existingCounterparty.Phone = counterparty.Phone;

        context.Counterparties.Update(existingCounterparty);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a counterparty by ID.
    /// </summary>
    /// <param name="id">Counterparty ID.</param>
    public async Task DeleteAsync(int id)
    {
        var counterparty = await context.Counterparties.FindAsync(id) ??
            throw new KeyNotFoundException($"Counterparty with Id {id} not found.");

        context.Counterparties.Remove(counterparty);
        await context.SaveChangesAsync();
    }
}