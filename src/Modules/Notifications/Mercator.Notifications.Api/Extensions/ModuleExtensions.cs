namespace Mercator.Notifications.Api.Extensions;

public static class ModuleExtensions
{
    public static void AddNotificationsModule(this IServiceCollection services)
    {
        services.AddEndpoints(typeof(ModuleExtensions).Assembly);
    }

    public static IApplicationBuilder MapNotificationsModule(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        app.MapEndpoints(routeGroupBuilder);
        return app;
    }
}

