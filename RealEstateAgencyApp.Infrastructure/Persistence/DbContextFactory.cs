using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace RealEstateAgencyApp.Infrastructure.Persistence;

/// <summary>
/// Factory for creating AppDbContext instances for EF Core design-time tools.
/// </summary>
public class DbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    /// <summary>
    /// Creates AppDbContext with configuration from AppSettings.json.
    /// </summary>
    public AppDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("AppSettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("mysqldb");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Connection string 'mysqldb' not found.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(
            connectionString,
            new MySqlServerVersion(new Version(9, 4, 0))
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}