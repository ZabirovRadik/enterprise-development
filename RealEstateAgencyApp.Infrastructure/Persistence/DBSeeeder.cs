using Microsoft.EntityFrameworkCore;
using RealEstateAgencyApp.Domain.DataSeeders;

namespace RealEstateAgencyApp.Infrastructure.Persistence;

/// <summary>
/// Provides methods for seeding the database with initial test data.
/// Contains separate methods for seeding each entity type and a combined method for all data.
/// </summary>
public static class DbSeeder
{
    /// <summary>
    /// Seeds the database with real estate objects if the table is empty.
    /// Also resets the auto-increment counter to continue from the next available ID.
    /// </summary>
    /// <param name="context">The database context to seed data into.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task SeedEstatesAsync(DBContext context)
    {
        if (!context.RealEstateObjects.Any())
        {
            var dataSeeder = new DataSeeder();
            context.RealEstateObjects.AddRange(dataSeeder.Estates);
            await context.SaveChangesAsync();
        }

        var next = (await context.RealEstateObjects.MaxAsync(e => (int?)e.Id) ?? 0) + 1;
        await context.Database.ExecuteSqlRawAsync(
            $"ALTER TABLE `RealEstateObjects` AUTO_INCREMENT = {next};"
        );
    }

    /// <summary>
    /// Seeds the database with counterparties if the table is empty.
    /// Also resets the auto-increment counter to continue from the next available ID.
    /// </summary>
    /// <param name="context">The database context to seed data into.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task SeedCounterpartiesAsync(DBContext context)
    {
        if (!context.Counterparties.Any())
        {
            var dataSeeder = new DataSeeder();
            context.Counterparties.AddRange(dataSeeder.Counterpaties);
            await context.SaveChangesAsync();
        }

        var next = (await context.Counterparties.MaxAsync(c => (int?)c.Id) ?? 0) + 1;
        await context.Database.ExecuteSqlRawAsync(
            $"ALTER TABLE `Counterparties` AUTO_INCREMENT = {next};"
        );
    }

    /// <summary>
    /// Seeds the database with requests if the table is empty.
    /// Also resets the auto-increment counter to continue from the next available ID.
    /// Requires that counterparties and real estate objects are seeded first.
    /// </summary>
    /// <param name="context">The database context to seed data into.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task SeedRequestsAsync(DBContext context)
    {
        if (!context.Requests.Any())
        {
            var dataSeeder = new DataSeeder();
            context.Requests.AddRange(dataSeeder.Requests);
            await context.SaveChangesAsync();
        }

        var next = (await context.Requests.MaxAsync(r => (int?)r.Id) ?? 0) + 1;
        await context.Database.ExecuteSqlRawAsync(
            $"ALTER TABLE `Requests` AUTO_INCREMENT = {next};"
        );
    }

    /// <summary>
    /// Performs complete database seeding in the correct order to maintain referential integrity.
    /// Order: Counterparties → RealEstateObjects → Requests.
    /// </summary>
    /// <param name="context">The database context to seed data into.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public static async Task SeedAllAsync(DBContext context)
    {
        await SeedCounterpartiesAsync(context);
        await SeedEstatesAsync(context);
        await SeedRequestsAsync(context);
    }
}