namespace Mercator.Payments.Api.Extensions;

public static class ModuleExtensions
{
    public static void AddPaymentsModule(this IServiceCollection services)
    {
        services.AddEndpoints(typeof(ModuleExtensions).Assembly);
    }

    public static IApplicationBuilder MapPaymentsModule(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        app.MapEndpoints(routeGroupBuilder);
        return app;
    }
}
