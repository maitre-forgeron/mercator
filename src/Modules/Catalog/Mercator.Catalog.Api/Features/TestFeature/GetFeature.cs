using Mercator.BuildingBlocks.Application.Execution;
using Mercator.Catalog.Api.Abstractions;
using Mercator.Catalog.Application.Features;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Mercator.Catalog.Api.Features.TestFeature;

public class GetFeature : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/features/{id:guid}", async Task<Results<Ok<FeatureDto>, BadRequest>> (
            Guid id,
            IQueryBus queryBus,
            CancellationToken ct) =>
        {
            var dto = await queryBus.Execute<GetFeatureQuery, FeatureDto?>(
                new GetFeatureQuery(id),
                ct);


            return TypedResults.Ok(dto);
        });
    }

    private sealed record AddFeatureRequest(string Name);
    private sealed record AddFeatureResponse(Guid Id);
}