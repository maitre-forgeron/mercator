using Mercator.BuildingBlocks.Application.Validation;

namespace Mercator.Identity.Core.Application.Register;

public class RegisterCommandValidator : IValidator<RegisterCommand>
{
    public void Validate(RegisterCommand request)
    {
        var errors = new List<string>();
        var r = request.Request;

        if (string.IsNullOrWhiteSpace(r.FirstName))
        {
            errors.Add("First name is required.");
        }
        if (string.IsNullOrWhiteSpace(r.LastName))
        {
            errors.Add("Last name is required.");
        }
        if (string.IsNullOrWhiteSpace(r.PersonalNumber))
        {
            errors.Add("Personal number is required.");
        }
        if (string.IsNullOrWhiteSpace(r.MobileNumber))
        {
            errors.Add("Mobile number is required.");
        }
        if (string.IsNullOrWhiteSpace(r.Email))
        {
            errors.Add("Email is required.");
        }
        if (string.IsNullOrWhiteSpace(r.Password) || r.Password.Length < 6)
        {
            errors.Add("Password must be at least 6 characters long.");
        }
            
        if (errors.Count > 0)
            throw new ValidationException(errors);
    }
}