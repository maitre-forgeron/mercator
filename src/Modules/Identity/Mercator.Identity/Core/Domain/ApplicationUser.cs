using Microsoft.AspNetCore.Identity;

namespace Mercator.Identity.Core.Domain;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string PersonalNumber { get; set; } = default!;
}
