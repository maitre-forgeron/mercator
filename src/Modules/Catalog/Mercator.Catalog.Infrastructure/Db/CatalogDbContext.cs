using Mercator.Catalog.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Mercator.Catalog.Infrastructure.Db;

public class CatalogDbContext : DbContext, ICatalogDbContext
{
    //Add DbSets for your entities here
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }
}