namespace Mercator.Catalog.Application.Abstractions;

public interface ICatalogDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    //Add DbSets for your entities here
}
