using Microsoft.EntityFrameworkCore;
using RealEstateAgencyApp.Domain.DataSeeders;

namespace RealEstateAgencyApp.Infrastructure.Persistence;

/// <summary>
/// Seeds the database with initial data for real estate objects, counterparties, and requests.
/// Works with MySQL.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedEstatesAsync(AppDbContext context)
    {
        if (!context.RealEstateObjects.Any())
        {
            var dataSeeder = new DataSeeder();
            context.RealEstateObjects.AddRange(dataSeeder.Estates);
            await context.SaveChangesAsync();
        }

        await context.Database.ExecuteSqlRawAsync(
            @"ALTER TABLE `RealEstateObjects` AUTO_INCREMENT = 
              (SELECT COALESCE(MAX(`Id`), 0) + 1 FROM `RealEstateObjects`);"
        );
    }

    public static async Task SeedCounterpartiesAsync(AppDbContext context)
    {
        if (!context.Counterparties.Any())
        {
            var dataSeeder = new DataSeeder();
            context.Counterparties.AddRange(dataSeeder.Counterpaties);
            await context.SaveChangesAsync();
        }

        await context.Database.ExecuteSqlRawAsync(
            @"ALTER TABLE `Counterparties` AUTO_INCREMENT = 
              (SELECT COALESCE(MAX(`Id`), 0) + 1 FROM `Counterparties`);"
        );
    }

    public static async Task SeedRequestsAsync(AppDbContext context)
    {
        if (!context.Requests.Any())
        {
            var dataSeeder = new DataSeeder();
            context.Requests.AddRange(dataSeeder.Requests);
            await context.SaveChangesAsync();
        }

        await context.Database.ExecuteSqlRawAsync(
            @"ALTER TABLE `Requests` AUTO_INCREMENT = 
              (SELECT COALESCE(MAX(`Id`), 0) + 1 FROM `Requests`);"
        );
    }

    /// <summary>
    /// ¬ыполн€ет все сиды в правильном пор€дке (Counterparties -> Estates -> Requests)
    /// </summary>
    public static async Task SeedAllAsync(AppDbContext context)
    {
        await SeedCounterpartiesAsync(context);
        await SeedEstatesAsync(context);
        await SeedRequestsAsync(context);
    }
}
