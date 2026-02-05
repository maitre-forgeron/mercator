using Mercator.Catalog.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Mercator.Bootstrapper.Helpers;

internal sealed class DbInitialiser
{
    private readonly CatalogDbContext _catalogContext;

    public DbInitialiser(CatalogDbContext catalogContext)
    {
        _catalogContext = catalogContext;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_catalogContext.Database.IsNpgsql())
            {
                await _catalogContext.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            // Log the exception (you can use your preferred logging framework here)
            throw;
        }
    }
}
