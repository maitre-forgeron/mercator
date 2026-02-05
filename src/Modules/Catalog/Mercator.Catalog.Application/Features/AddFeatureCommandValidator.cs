using Mercator.BuildingBlocks.Application.Validation;
using Mercator.Catalog.Application.Features;

namespace Mercator.Catalog.Api.Features.TestFeature.AddFeature;

public sealed class AddFeatureCommandValidator : IValidator<AddFeatureCommand>
{
    public void Validate(AddFeatureCommand request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.Name))
            errors.Add("Name is required.");

        var name = request.Name?.Trim() ?? string.Empty;

        if (name.Length is < 2 or > 100)
            errors.Add("Name must be between 2 and 100 characters.");

        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}
