using Mercator.Bootstrapper.Application;
using Mercator.Bootstrapper.Helpers;
using Mercator.BuildingBlocks.Application.Abstractions.Pipeline;
using Mercator.BuildingBlocks.Application.Behaviors;
using Mercator.BuildingBlocks.Application.Execution;
using Mercator.Catalog.Api.Extensions;
using Mercator.Identity.Core.Infrastructure.Extensions;
using Mercator.Identity.Extensions;
using Mercator.Logistics.Api.Extensions;
using Mercator.Notifications.Api.Extensions;
using Mercator.Orders.Api.Extensions;
using Mercator.Payments.Api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;

namespace Mercator.Bootstrapper.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMercatorModules(this IServiceCollection services, IConfiguration configuraiton)
    {
        services.AddOrdersModule();
        services.AddPaymentsModule();
        services.AddNotificationsModule();
        services.AddLogisticsModule();
        services.AddCatalogModule(configuraiton);
        services.AddIdentityModule(configuraiton);

        return services;
    }

    public static IServiceCollection AddGlobalConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApiExloration();
        services.AddMessageBus();

        services.AddGlobalInfrastructure(configuration);

        return services;
    }

    private static IServiceCollection AddGlobalInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<DbInitialiser>();

        services.AddMercatorAuh(configuration);

        return services;
    }

    private static IServiceCollection AddMercatorAuh(this IServiceCollection services, IConfiguration configuration)
    {
        var jwt = configuration.GetSection("Auth:Jwt");
        var signingKey = jwt["SigningKey"]!;

        services
          .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(options =>
          {
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidIssuer = jwt["Issuer"],

                  ValidateAudience = true,
                  ValidAudience = jwt["Audience"],

                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),

                  ValidateLifetime = true,
                  ClockSkew = TimeSpan.FromSeconds(30)
              };
          });

        services.AddAuthorization();

        return services;
    }

    private static IServiceCollection AddApiExloration(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

        services.AddEndpointsApiExplorer();

        return services;
    }

    private static IServiceCollection AddMessageBus(this IServiceCollection services)
    {
        services.AddScoped<ICommandBus, DefaultCommandBus>();
        services.AddScoped<IQueryBus, DefaultQueryBus>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder MapMercatorModules(this WebApplication app)
    {
        app.MapOrdersModule(app.MapGroup("/orders").WithTags("Orders"));
        app.MapPaymentsModule(app.MapGroup("/payments").WithTags("Payments"));
        app.MapNotificationsModule(app.MapGroup("/notifications").WithTags("Notifications"));
        app.MapLogisticsModule(app.MapGroup("/logistics").WithTags("Logistics"));
        app.MapCatalogModule(app.MapGroup("/catalog").WithTags("Catalog"));
        app.MapIdentityModule(app.MapGroup("/identity").WithTags("Identity"));

        return app;
    }

    public static IApplicationBuilder UseApiExploration(this WebApplication app)
    {
        app.MapOpenApi();

        app.MapScalarApiReference(options =>
        {
            options.WithTheme(ScalarTheme.Kepler);
        });

        return app;
    }

    public static IApplicationBuilder UseMercatorAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
