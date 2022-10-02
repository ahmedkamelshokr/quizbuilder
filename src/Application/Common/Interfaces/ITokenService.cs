namespace Application.Common.Interfaces
{
    public interface ITokenService {
        TokenModel CreateAuthenticationToken(string userId, string userName, IEnumerable<(string claimType, string claimValue)> customClaims = null);
    }
}
