namespace Mercator.Orders.Api.Extensions;

public static class ModuleExtensions
{
    public static void AddOrdersModule(this IServiceCollection services)
    {
        services.AddEndpoints(typeof(ModuleExtensions).Assembly);
    }

    public static IApplicationBuilder MapOrdersModule(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        app.MapEndpoints(routeGroupBuilder);
        return app;
    }
}