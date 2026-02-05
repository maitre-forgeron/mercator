using Mercator.BuildingBlocks.Application.Abstractions.Commands;
using Mercator.BuildingBlocks.Application.Abstractions.Queries;
using Mercator.BuildingBlocks.Application.Validation;
using Mercator.Identity.Core.Application.Contracts;
using Mercator.Identity.Core.Application.Login;
using Mercator.Identity.Core.Application.Logout;
using Mercator.Identity.Core.Application.Me;
using Mercator.Identity.Core.Application.Refresh;
using Mercator.Identity.Core.Application.Register;
using Mercator.Identity.Core.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Mercator.Identity.Extensions;

public static class ModuleExtensions
{
    public static void AddIdentityModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpoints(typeof(ModuleExtensions).Assembly);

        services.AddApplicationServices();
        services.AddIdentityInfrastructure(configuration);
    }

    private static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<RegisterCommand, AuthResponse>, RegisterCommandHandler>();
        services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();

        services.AddScoped<ICommandHandler<RefreshCommand, AuthResponse>, RefreshCommandHandler>();
        services.AddScoped<ICommandHandler<LoginCommand, AuthResponse>, LoginCommandHandler>();
        services.AddScoped<ICommandHandler<LogoutCommand, bool>, LogoutCommandHandler>();

        services.AddScoped<IQueryHandler<GetMeQuery, MeResponse>, GetMeQueryHandler>();
    }

    public static IApplicationBuilder MapIdentityModule(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        app.MapEndpoints(routeGroupBuilder);
        return app;
    }
}
