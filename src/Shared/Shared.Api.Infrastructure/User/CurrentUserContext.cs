using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using SharedKernel;

namespace Shared.Api.Infrastructure.User;

public class CurrentUserContext(IHttpContextAccessor httpContextAccessor) : ICurrentUserContext
{
    public string UserId => httpContextAccessor.HttpContext?.User
        ?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value!;
    
    public Guid UserIdGuid =>
        Guid.TryParse(httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value, out var g)
            ? g
            : Guid.Empty;
    
    
}