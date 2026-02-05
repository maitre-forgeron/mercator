using Mercator.BuildingBlocks.Application.Abstractions.Queries;

namespace Mercator.BuildingBlocks.Application.Execution;

public interface IQueryBus
{
    Task<TResult> Execute<TQuery, TResult>(TQuery query, CancellationToken ct = default)
        where TQuery : IQuery<TResult>;
}