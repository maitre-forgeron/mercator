using Mercator.BuildingBlocks.Application.Execution;
using Mercator.Identity.Abstractions;
using Mercator.Identity.Core.Application.Me;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Mercator.Identity.Features;

public class Me : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/me", async Task<Results<Ok<MeResponse>, UnauthorizedHttpResult>> (
            IQueryBus queryBus,
            HttpContext http,
            CancellationToken ct) =>
        {
            var result = await queryBus.Execute<GetMeQuery, MeResponse>(new GetMeQuery(http.User), ct);

            return TypedResults.Ok(result);
        })
            .WithName("Me")
            .WithSummary("Me endpoint")
            .RequireAuthorization();
    }
}