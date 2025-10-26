using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RealEstateAgencyApp.Infrastructure.Persistence;

/// <summary>
/// Design-time factory for creating AppDbContext instances for EF Core tools.
/// </summary>
public class DbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseMySql(
            "Server=localhost;Port=3306;User ID=root;Password=P@ssw0rd;Database=mysqldb",
            new MySqlServerVersion(new Version(9, 4, 0))
        );

        return new AppDbContext(optionsBuilder.Options);
    }
}
