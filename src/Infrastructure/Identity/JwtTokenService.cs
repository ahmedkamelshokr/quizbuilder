using Application.Common.Interfaces;
using Application.Common.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Identity
{
    public class JwtTokenService : ITokenService
    {
        private readonly AuthConfiguration _authConfiguration;

        public JwtTokenService(IOptions<AuthConfiguration> authConfiguration)
        {
            _authConfiguration = authConfiguration.Value;
        }

        public TokenModel CreateAuthenticationToken(string userId, string userName,
            IEnumerable<(string claimType, string claimValue)> customClaims = null)
        {
            var expiration = DateTime.UtcNow.AddMinutes(_authConfiguration.TokenValidityInMinutes);
             var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Iss, _authConfiguration.JwtIssuer),
                    new Claim(JwtRegisteredClaimNames.Aud, _authConfiguration.JwtAudience),
                    new Claim(JwtRegisteredClaimNames.UniqueName, userName),
                    new Claim(JwtRegisteredClaimNames.NameId, userId)
                }.Concat(
                    customClaims?.Select(x => new Claim(x.claimType, x.claimValue)) ?? Enumerable.Empty<Claim>())
                ),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_authConfiguration.Base64UrlDecodeSecretKey()), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new TokenModel(
                tokenType: "Bearer",
                accessToken: tokenString,
                expiresAt: expiration
            );
        }              
    }
}
