using Mercator.BuildingBlocks.Application.Abstractions.Queries;
using Mercator.Identity.Core.Domain;
using System.Security.Claims;

namespace Mercator.Identity.Core.Application.Me;

public sealed record MeResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string PersonalNumber,
    string MobileNumber);

public sealed record GetMeQuery(ClaimsPrincipal Principal) : IQuery<MeResponse>;

public sealed class GetMeQueryHandler : IQueryHandler<GetMeQuery, MeResponse>
{
    private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _users;

    public GetMeQueryHandler(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> users) => _users = users;

    public async Task<MeResponse> Handle(GetMeQuery query, CancellationToken ct)
    {
        var user = await _users.GetUserAsync(query.Principal)
                   ?? throw new UnauthorizedAccessException();

        return new MeResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.PersonalNumber,
            user.PhoneNumber!);
    }
}
