using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

public static class AuthInjection
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, AuthConfiguration authSettings)
    {
        // Prevents the mapping of sub claim into archaic SOAP NameIdentifier.
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

        services
            .AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
#if DEBUG
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = ctx => {
                    // Break here to debug JWT authentication.                            
                        return Task.FromResult(true);
                    }
                };
#endif

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authSettings.JwtIssuer,

                    ValidateAudience = true,
                    ValidAudience = authSettings.JwtAudience,

                       ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new  SymmetricSecurityKey(authSettings.Base64UrlDecodeSecretKey()),
                    ClockSkew = TimeSpan.FromMinutes(5),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    NameClaimType = JwtRegisteredClaimNames.NameId
                };
            });

        return services;
    }
}