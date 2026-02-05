namespace Mercator.Logistics.Api.Extensions;

public static class ModuleExtensions
{
    public static void AddLogisticsModule(this IServiceCollection services)
    {
        services.AddEndpoints(typeof(ModuleExtensions).Assembly);
    }

    public static IApplicationBuilder MapLogisticsModule(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        app.MapEndpoints(routeGroupBuilder);
        return app;
    }
}

