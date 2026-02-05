using Mercator.BuildingBlocks.Application.Abstractions.Commands;

namespace Mercator.BuildingBlocks.Application.Execution;

public interface ICommandBus
{
    Task<TResult> Execute<TCommand, TResult>(TCommand command, CancellationToken ct = default)
        where TCommand : ICommand<TResult>;
}
