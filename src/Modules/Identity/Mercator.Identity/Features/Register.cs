using Mercator.BuildingBlocks.Application.Execution;
using Mercator.Identity.Abstractions;
using Mercator.Identity.Core.Application.Contracts;
using Mercator.Identity.Core.Application.Register;
using Mercator.Identity.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Mercator.Identity.Features;

public class Register : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/register", async Task<Results<Ok<AuthResponse>, UnauthorizedHttpResult>> (
            RegisterRequest request,
            ICommandBus commandBus,
            HttpContext http,
            CancellationToken ct) =>
        {
            var authResponse = await commandBus.Execute<RegisterCommand, AuthResponse>(new RegisterCommand(request), ct);

            http.Response.Cookies.Append(RefreshCookie.Name, authResponse.RefreshToken, RefreshCookie.Build());

            return TypedResults.Ok(authResponse);
        })
            .WithName("Register")
            .WithSummary("User registration endpoint");
    }
}