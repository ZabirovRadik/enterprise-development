using Microsoft.EntityFrameworkCore;
using RealEstateAgencyApp.Domain.Entities;
using RealEstateAgencyApp.Domain.Entities.Enums;
using RealEstateAgencyApp.Domain.Interfaces;
using RealEstateAgencyApp.Infrastructure.Persistence;

namespace RealEstateAgencyApp.Infrastructure.Repositories;

/// <summary>
/// Repository for managing RealEstateObject entities.
/// Provides CRUD methods for real estate objects.
/// </summary>
/// <param name="context">The application's database context used for data access.</param>
public class RealEstateObjectRepository(AppDbContext context) : IRealEstateObjectRepository
{
    /// <summary>
    /// Gets all real estate objects.
    /// </summary>
    public async Task<IEnumerable<RealEstateObject>> GetAllAsync() =>
        await context.RealEstateObjects.ToListAsync();

    /// <summary>
    /// Gets a real estate object by its ID.
    /// </summary>
    /// <param name="id">Real estate object ID.</param>
    /// <returns>The <see cref="RealEstateObject"/> if found; otherwise, null.</returns>
    public async Task<RealEstateObject?> GetByIdAsync(int id) =>
        await context.RealEstateObjects.FirstOrDefaultAsync(e => e.Id == id);

    /// <summary>
    /// Gets a real estate object by cadastral number.
    /// </summary>
    /// <param name="cadastralNumber">Cadastral number.</param>
    /// <returns>The <see cref="RealEstateObject"/> if found; otherwise, null.</returns>
    public async Task<RealEstateObject?> GetByCadastralNumberAsync(string cadastralNumber) =>
        await context.RealEstateObjects.FirstOrDefaultAsync(e => e.CadastralNumber == cadastralNumber);

    /// <summary>
    /// Gets real estate objects by type.
    /// </summary>
    /// <param name="type">Type of real estate.</param>
    /// <returns>List of real estate objects of specified type.</returns>
    public async Task<IEnumerable<RealEstateObject>> GetByTypeAsync(RealEstateType type) =>
        await context.RealEstateObjects.Where(e => e.Type == type).ToListAsync();

    /// <summary>
    /// Gets real estate objects by purpose.
    /// </summary>
    /// <param name="purpose">Purpose of real estate.</param>
    /// <returns>List of real estate objects of specified purpose.</returns>
    public async Task<IEnumerable<RealEstateObject>> GetByPurposeAsync(RealEstatePurpose purpose) =>
        await context.RealEstateObjects.Where(e => e.Purpose == purpose).ToListAsync();

    /// <summary>
    /// Checks if a real estate object exists by ID.
    /// </summary>
    /// <param name="id">Real estate object ID.</param>
    /// <returns>True if exists; otherwise, false.</returns>
    public async Task<bool> ExistsByIdAsync(int id) =>
        await context.RealEstateObjects.AnyAsync(e => e.Id == id);

    /// <summary>
    /// Checks if a real estate object exists by cadastral number.
    /// </summary>
    /// <param name="cadastralNumber">Cadastral number.</param>
    /// <returns>True if exists; otherwise, false.</returns>
    public async Task<bool> ExistsByCadastralNumberAsync(string cadastralNumber) =>
        await context.RealEstateObjects.AnyAsync(e => e.CadastralNumber == cadastralNumber);

    /// <summary>
    /// Adds a new real estate object.
    /// </summary>
    /// <param name="realEstateObject">Real estate object to add.</param>
    public async Task AddAsync(RealEstateObject realEstateObject)
    {
        await context.RealEstateObjects.AddAsync(realEstateObject);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing real estate object.
    /// </summary>
    /// <param name="realEstateObject">Real estate object with updated data.</param>
    public async Task UpdateAsync(RealEstateObject realEstateObject)
    {
        var existingEstate = await context.RealEstateObjects.FindAsync(realEstateObject.Id) ??
            throw new KeyNotFoundException($"Real estate object with Id {realEstateObject.Id} not found.");

        existingEstate.Type = realEstateObject.Type;
        existingEstate.Purpose = realEstateObject.Purpose;
        existingEstate.CadastralNumber = realEstateObject.CadastralNumber;
        existingEstate.Address = realEstateObject.Address;
        existingEstate.Floors = realEstateObject.Floors;
        existingEstate.Area = realEstateObject.Area;
        existingEstate.Rooms = realEstateObject.Rooms;
        existingEstate.CeilingHeight = realEstateObject.CeilingHeight;
        existingEstate.Floor = realEstateObject.Floor;
        existingEstate.HasEncumbrances = realEstateObject.HasEncumbrances;

        context.RealEstateObjects.Update(existingEstate);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a real estate object by ID.
    /// </summary>
    /// <param name="id">Real estate object ID.</param>
    public async Task DeleteAsync(int id)
    {
        var estate = await context.RealEstateObjects.FindAsync(id) ??
            throw new KeyNotFoundException($"Real estate object with Id {id} not found.");

        context.RealEstateObjects.Remove(estate);
        await context.SaveChangesAsync();
    }
}