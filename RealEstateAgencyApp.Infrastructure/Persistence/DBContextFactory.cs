using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace RealEstateAgencyApp.Infrastructure.Persistence;

/// <summary>
/// Design-time factory for creating DBContext instances for EF Core tools.
/// </summary>
public class DBContextFactory : IDesignTimeDbContextFactory<DBContext>
{
    public DBContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DBContext>();

        optionsBuilder.UseMySql(
            "Server=localhost;Port=3306;User ID=root;Password=P@ssw0rd;Database=mysqldb",
            new MySqlServerVersion(new Version(9, 4, 0))
);

        return new DBContext(optionsBuilder.Options);
    }
}
