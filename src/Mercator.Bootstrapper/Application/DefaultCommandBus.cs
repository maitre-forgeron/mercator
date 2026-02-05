
using Mercator.BuildingBlocks.Application.Abstractions.Commands;
using Mercator.BuildingBlocks.Application.Abstractions.Pipeline;
using Mercator.BuildingBlocks.Application.Execution;

namespace Mercator.Bootstrapper.Application;

internal sealed class DefaultCommandBus(IServiceProvider sp) : ICommandBus
{
    public async Task<TResult> Execute<TCommand, TResult>(TCommand command, CancellationToken ct = default)
        where TCommand : ICommand<TResult>
    {
        var behaviors = sp.GetServices<IPipelineBehavior<TCommand, TResult>>()
            .OrderBy(b => (b as IOrderedPipelineBehavior)?.Order ?? 1000)
            .ToArray();
        var handler = sp.GetRequiredService<ICommandHandler<TCommand, TResult>>();

        RequestHandlerDelegate<TResult> terminal = () => handler.Handle(command, ct);

        var pipeline = behaviors.Reverse()
            .Aggregate(terminal, (next, behavior) => () => behavior.Handle(command, ct, next));

        return await pipeline();
    }
}
