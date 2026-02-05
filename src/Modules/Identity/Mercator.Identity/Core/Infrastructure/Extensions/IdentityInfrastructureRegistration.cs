using Mercator.Identity.Core.Application;
using Mercator.Identity.Core.Domain;
using Mercator.Identity.Core.Infrastructure.Models;
using Mercator.Identity.Core.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Mercator.Identity.Core.Infrastructure.Extensions;

public static class IdentityInfrastructureRegistration
{
    public static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        IConfiguration config)
    {
        var connectionString = config.GetConnectionString("mercator");
        Console.WriteLine(connectionString);
        services.AddDbContext<IdentityDbContext>(opt =>
            opt.UseNpgsql(connectionString));

        services
            .AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 8;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            })
            .AddRoles<Microsoft.AspNetCore.Identity.IdentityRole<Guid>>()
            .AddEntityFrameworkStores<IdentityDbContext>();

        services.Configure<JwtOptions>(config.GetSection("Auth:Jwt"));
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
