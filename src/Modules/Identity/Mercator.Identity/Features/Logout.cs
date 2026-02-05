using Mercator.BuildingBlocks.Application.Execution;
using Mercator.Identity.Abstractions;
using Mercator.Identity.Core.Application.Contracts;
using Mercator.Identity.Core.Application.Login;
using Mercator.Identity.Core.Application.Logout;
using Mercator.Identity.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Mercator.Identity.Features;

public class Logout : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/logout", async Task<Results<Ok<AuthResponse>, NoContent>> (
            ICommandBus commandBus,
            HttpContext http,
            CancellationToken ct) =>
        {
            if (http.Request.Cookies.TryGetValue(RefreshCookie.Name, out var refreshPlain) &&
                !string.IsNullOrWhiteSpace(refreshPlain))
            {
                await commandBus.Execute<LogoutCommand, bool>(new LogoutCommand(refreshPlain), ct);
            }

            http.Response.Cookies.Delete(RefreshCookie.Name, RefreshCookie.Build());

            return TypedResults.NoContent();
        })
            .WithName("Logout")
            .WithSummary("User logout endpoint");
    }
}
