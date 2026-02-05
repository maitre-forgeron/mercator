using Mercator.BuildingBlocks.Application.Execution;
using Mercator.Identity.Abstractions;
using Mercator.Identity.Core.Application.Contracts;
using Mercator.Identity.Core.Application.Login;
using Mercator.Identity.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Mercator.Identity.Features;

public class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async Task<Results<Ok<AuthResponse>, UnauthorizedHttpResult>> (
            LoginRequest request,
            ICommandBus commandBus,
            HttpContext http,
            CancellationToken ct) =>
        {
            var authResponse = await commandBus.Execute<LoginCommand, AuthResponse>(new LoginCommand(request), ct);

            if (authResponse is null)
            {
                return TypedResults.Unauthorized();
            }

            http.Response.Cookies.Append(RefreshCookie.Name, authResponse.RefreshToken, RefreshCookie.Build());

            return TypedResults.Ok(authResponse);
        })
            .WithName("Login")
            .WithSummary("User login endpoint");
    }
}