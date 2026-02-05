namespace Mercator.BuildingBlocks.Application.Validation;

public sealed class ValidationException(IReadOnlyList<string> errors)
    : Exception("Validation failed")
{
    public IReadOnlyList<string> Errors { get; } = errors;
}
