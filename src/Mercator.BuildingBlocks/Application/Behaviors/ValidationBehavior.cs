using Mercator.BuildingBlocks.Application.Abstractions.Pipeline;
using Mercator.BuildingBlocks.Application.Validation;

namespace Mercator.BuildingBlocks.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResult>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResult>, IOrderedPipelineBehavior
{
    public int Order => 0;

    public Task<TResult> Handle(
        TRequest request,
        CancellationToken ct,
        RequestHandlerDelegate<TResult> next)
    {
        if (!validators.Any())
            return next();

        var errors = new List<string>();

        foreach (var validator in validators)
        {
            try
            {
                validator.Validate(request);
            }
            catch (ValidationException ex)
            {
                errors.AddRange(ex.Errors);
            }
        }

        if (errors.Count > 0)
            throw new ValidationException(errors);

        return next();
    }
}