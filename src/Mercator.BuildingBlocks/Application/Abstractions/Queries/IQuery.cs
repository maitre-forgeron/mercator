namespace Mercator.BuildingBlocks.Application.Abstractions.Queries;

public interface IQuery<out TResult>;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery query, CancellationToken ct = default);
}