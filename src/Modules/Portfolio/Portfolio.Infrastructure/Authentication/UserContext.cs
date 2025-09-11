using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Portfolio.Application.Abstractions.Authentication;

namespace Portfolio.Infrastructure.Authentication;

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public string UserId => httpContextAccessor.HttpContext?.User
        ?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value!;

    public Guid UserID => Guid.TryParse(UserId, out var id) ? id : Guid.Empty;
}