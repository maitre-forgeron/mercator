using Mercator.BuildingBlocks.Application.Abstractions.Commands;
using Mercator.BuildingBlocks.Application.Abstractions.Pipeline;
using Mercator.BuildingBlocks.Application.Abstractions.Queries;
using Mercator.BuildingBlocks.Application.Behaviors;
using Mercator.BuildingBlocks.Application.Validation;
using Mercator.Catalog.Api.Features.TestFeature.AddFeature;
using Mercator.Catalog.Application.Abstractions;
using Mercator.Catalog.Application.Features;
using Mercator.Catalog.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Mercator.Catalog.Api.Extensions;

public static class ModuleExtensions
{
    public static void AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpoints(typeof(ModuleExtensions).Assembly);

        services.AddApplicationServices();
        services.AddInfrastructureServices(configuration);
    }

    private static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<AddFeatureCommand, Guid>, AddFeatureHandler>();
        services.AddScoped<IValidator<AddFeatureCommand>, AddFeatureCommandValidator>();
     
        services.AddScoped<IQueryHandler<GetFeatureQuery, FeatureDto?>, GetFeatureQueryHandler>();
    }

    private static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ICatalogDbContext, CatalogDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("mercator"));
        });
    }

    public static IApplicationBuilder MapCatalogModule(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        app.MapEndpoints(routeGroupBuilder);
        return app;
    }
}
