using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YoutubeClone.Application.Interfaces.Services;
using YoutubeClone.Application.Models.Helpers;
using YoutubeClone.Shared;
using YoutubeClone.Shared.Constants;
using YoutubeClone.Shared.Helpers;

namespace YoutubeClone.Application.Helpers
{
    public class TokenHelper
    {
        public static readonly Random rnd = new();
        public static string Create(Guid userId, List<string> roles, IConfiguration configuration, ICacheService cache)
        {
            var tokenConfiguration = Configuration(configuration);
            var signingCredentials = new SigningCredentials(tokenConfiguration.SecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimsConstants.USER_ID, userId.ToString()),
                new Claim(ClaimTypes.Role, roles[0])
            };

            var securityToken = new JwtSecurityToken(
                audience: tokenConfiguration.Audience,
                issuer: tokenConfiguration.Issuer,
                expires: tokenConfiguration.ExpirationDateTime,
                signingCredentials: signingCredentials,
                claims: claims
                );
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            var cacheKey = CacheHelper.AuthTokenCreation(token, tokenConfiguration.ExpirationTimeSpan);
            cache.Create(cacheKey.Key, cacheKey.Expiration, token);

            return token;
        }

        public static string CreateRefresh(Guid userId, IConfiguration configuration, ICacheService cacheService)
        {
            var token = Generate.RandomText(100);
            var cacheKey = CacheHelper.AuthRefreshTokenCreation(token, configuration);

            cacheService.Create(cacheKey.Key, cacheKey.Expiration, new RefreshToken
            {
                UserId = userId,
                ExpirationInDays = cacheKey.Expiration
            });

            return token;
        }

        public static TokenConfiguration Configuration(IConfiguration configuration)
        {
            var issuer = Environment.GetEnvironmentVariable(ConfigurationConstants.JWT_ISSUER)
                ?? configuration[ConfigurationConstants.JWT_ISSUER]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.JWT_ISSUER));

            var audience = Environment.GetEnvironmentVariable(ConfigurationConstants.JWT_AUDIENCE)
                ?? configuration[ConfigurationConstants.JWT_AUDIENCE]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.JWT_AUDIENCE));

            var privateKey = Environment.GetEnvironmentVariable(ConfigurationConstants.JWT_PRIVATE_KEY)
                ?? configuration[ConfigurationConstants.JWT_PRIVATE_KEY]
                ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.JWT_PRIVATE_KEY));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey));

            var now = DateTimeHelper.UtcNow();
            var randomExpiration = rnd.Next(Convert.ToInt32(configuration[ConfigurationConstants.JWT_EXPIRATION_IN_MINUTES_MIN] ?? "1"), Convert.ToInt32(configuration[ConfigurationConstants.JWT_EXPIRATION_IN_MINUTES_MAX] ?? "5"));
            var timespanExpiration = TimeSpan.FromMinutes(randomExpiration);
            var datetimeExpiration = now.Add(TimeSpan.FromMinutes(randomExpiration));

            return new TokenConfiguration
            {
                Issuer = issuer,
                Audience = audience,
                SecurityKey = securityKey,
                ExpirationDateTime = datetimeExpiration,
                ExpirationTimeSpan = timespanExpiration
            };
        }
    }
}
