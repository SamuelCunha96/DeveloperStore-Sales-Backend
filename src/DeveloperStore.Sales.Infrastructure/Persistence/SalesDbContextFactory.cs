using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DeveloperStore.Sales.Infrastructure.Persistence;

public class SalesDbContextFactory : IDesignTimeDbContextFactory<SalesDbContext>
{
    public SalesDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<SalesDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=developerstore_sales;Username=postgres;Password=postgres")
            .Options;

        return new SalesDbContext(options);
    }
}
