using System.Security.Claims;

namespace Portfolio.Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string Create(IEnumerable<Claim> claims);
}