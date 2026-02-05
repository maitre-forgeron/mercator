namespace Mercator.BuildingBlocks.Application.Validation;

public interface IValidator<in TRequest>
{
    void Validate(TRequest request);
}