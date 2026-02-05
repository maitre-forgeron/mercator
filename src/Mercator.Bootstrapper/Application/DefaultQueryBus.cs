using Mercator.BuildingBlocks.Application.Abstractions.Pipeline;
using Mercator.BuildingBlocks.Application.Abstractions.Queries;
using Mercator.BuildingBlocks.Application.Execution;

namespace Mercator.Bootstrapper.Application;

internal sealed class DefaultQueryBus(IServiceProvider sp) : IQueryBus
{
    public async Task<TResult> Execute<TQuery, TResult>(TQuery query, CancellationToken ct = default)
        where TQuery : IQuery<TResult>
    {
        var behaviors = sp.GetServices<IPipelineBehavior<TQuery, TResult>>().ToArray();
        var handler = sp.GetRequiredService<IQueryHandler<TQuery, TResult>>();

        RequestHandlerDelegate<TResult> terminal = () => handler.Handle(query, ct);

        var pipeline = behaviors.Reverse()
            .Aggregate(terminal, (next, behavior) => () => behavior.Handle(query, ct, next));

        return await pipeline();
    }
}