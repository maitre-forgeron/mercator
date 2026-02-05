using Mercator.BuildingBlocks.Application.Execution;
using Mercator.Catalog.Api.Abstractions;
using Mercator.Catalog.Application.Features;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Mercator.Catalog.Api.Features.TestFeature.AddFeature;

public class AddFeature : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/features", async Task<Results<Created<AddFeatureResponse>, BadRequest>> (
            AddFeatureRequest request,
            ICommandBus commandBus,
            CancellationToken ct) =>
        {
            // Add feature to db via command
            var id = await commandBus.Execute<AddFeatureCommand, Guid>(
                new AddFeatureCommand(request.Name),
                ct);


            return TypedResults.Created($"/api/catalog/features/{id}", new AddFeatureResponse(id));
        })
            .WithName("AddFeature")
            .WithSummary("Shows example to add a feature");
    }

    private sealed record AddFeatureRequest(string Name);
    private sealed record AddFeatureResponse(Guid Id);
}
