using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CleanArchitecture.Infrastructure.Data;

/// <summary>
/// Used by dotnet-ef at design time (migrations) so it can create an AppDbContext
/// without running the full application host.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CleanArchitectureDb;Trusted_Connection=True;")
            .Options;

        return new AppDbContext(options);
    }
}
