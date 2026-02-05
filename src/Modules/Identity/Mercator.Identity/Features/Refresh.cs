using Mercator.BuildingBlocks.Application.Execution;
using Mercator.Identity.Abstractions;
using Mercator.Identity.Core.Application.Contracts;
using Mercator.Identity.Core.Application.Login;
using Mercator.Identity.Core.Application.Me;
using Mercator.Identity.Core.Application.Refresh;
using Mercator.Identity.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Mercator.Identity.Features;

public class Refresh : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/refresh", async Task<Results<Ok<AuthResponse>, UnauthorizedHttpResult>> (
            ICommandBus commandBus,
            HttpContext http,
            CancellationToken ct) =>
        {
            if (!http.Request.Cookies.TryGetValue(RefreshCookie.Name, out var refreshPlain) ||
                string.IsNullOrWhiteSpace(refreshPlain))
            {
                return TypedResults.Unauthorized();
            }

            var authResponse = await commandBus.Execute<RefreshCommand, AuthResponse>(new RefreshCommand(refreshPlain), ct);

            http.Response.Cookies.Append(RefreshCookie.Name, authResponse.RefreshToken, RefreshCookie.Build());

            return TypedResults.Ok(authResponse);
        })
            .WithName("Refresh")
            .WithSummary("User refresh token endpoint");
    }
}