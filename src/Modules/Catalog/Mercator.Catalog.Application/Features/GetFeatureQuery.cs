using Mercator.BuildingBlocks.Application.Abstractions.Queries;

namespace Mercator.Catalog.Application.Features;

public sealed record GetFeatureQuery(Guid Id) : IQuery<FeatureDto?>;

public sealed record FeatureDto(Guid Id, string Name);

public sealed class GetFeatureQueryHandler()
    : IQueryHandler<GetFeatureQuery, FeatureDto?>
{
    public async Task<FeatureDto?> Handle(GetFeatureQuery query, CancellationToken ct = default)
    {
        //do request to db

        return new FeatureDto(query.Id, "Test");
    }
}