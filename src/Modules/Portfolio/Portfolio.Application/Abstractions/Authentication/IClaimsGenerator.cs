using System.Security.Claims;

namespace Portfolio.Application.Abstractions.Authentication;

public interface IClaimsGenerator
{
    Task<List<Claim>> GenerateAsync(Guid userId, string idp, CancellationToken ct = default);
}
