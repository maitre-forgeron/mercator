namespace Mercator.BuildingBlocks.Application.Abstractions.Pipeline;

public interface IPipelineBehavior<TRequest, TResult>
{
    Task<TResult> Handle(TRequest request, CancellationToken ct, RequestHandlerDelegate<TResult> next);
}

public delegate Task<TResult> RequestHandlerDelegate<TResult>();

public interface IPipelineBehavior<TRequest>
{
    Task Handle(TRequest request, CancellationToken ct, RequestHandlerDelegate next);
}

public delegate Task RequestHandlerDelegate();

public interface IOrderedPipelineBehavior
{
    int Order { get; }
}